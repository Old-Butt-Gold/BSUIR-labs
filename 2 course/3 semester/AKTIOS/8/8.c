#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <time.h>
#include <signal.h>
#include <sys/wait.h>
#include <stdbool.h>

#define PROC_COUNT 9

static int i = 1;
static int USR1 = 0, USR2 = 0;
static bool flag = false;
pid_t arrpid[PROC_COUNT] = {0, 0, 0, 0, 0, 0, 0, 0, 0};

void printProcessInfo(int signum)
{
    printf("%d process, pid: %d, ppid: %d, got a signal %s, time %ld\n\n", i, getpid(), getppid(), (signum == SIGUSR1) ? "SIGUSR1" : "SIGUSR2", clock());
}

void sendSignalToProcess(int processIndex, int signal, int SIGCOUNT)
{
    for (int h = 1; h <= SIGCOUNT; h++)
    {
        kill(arrpid[processIndex], signal);
        printf(">> %d process, pid: %d, ppid: %d, send signal %s to process %d, time %ld\n\n", i, getpid(), getppid(), (signal == SIGUSR1) ? "SIGUSR1" : "SIGUSR2", processIndex, clock());
        usleep(100000);
    }
    kill(arrpid[processIndex], SIGTERM);
}

void sendSignalToGroup(int processIndex, int signal, int SIGCOUNT)
{
    setpgid(arrpid[4], arrpid[4]);
    setpgid(arrpid[5], arrpid[4]);
    pid_t group = getpgid(arrpid[4]);

    for (int h = 1; h <= SIGCOUNT; h++)
    {
        killpg(group, signal);
        printf(">> %d process, pid: %d, ppid: %d, send signal %s to group %d, time %ld\n\n", i, getpid(), getppid(), (signal == SIGUSR1) ? "SIGUSR1" : "SIGUSR2", group, clock());
        usleep(100000);
    }
    killpg(group, SIGTERM);
}

void signalHandler(int signum)
{
    usleep(clock());
    switch (signum)
    {
        case SIGUSR1:
            printProcessInfo(signum);
            USR1++;
            break;
        case SIGUSR2:
            printProcessInfo(signum);
            USR2++;
            break;
        default:
            flag = true;
            break;
    }
}

void waitForChildProcesses()
{
    for (int i = 0; i < PROC_COUNT; i++)
        wait(NULL);
    printf("=== %d process, pid: %d, ppid: %d, ended work after #%d signal SIGUSR1 and #%d signal SIGUSR2\n\n", i, getpid(), getppid(), USR1, USR2);
}

void createChildProcesses(FILE *filepid, const int SIGNUM)
{
    pid_t pid = fork();

    if (pid == 0)
    {
        arrpid[i] = getpid();
        // 1 -> (2, 3, 4, 5, 6, 7)
        for (int j = 2; j <= 7; j++)
        {
            pid = fork();

            if (pid == 0)
            {
                i = j;
                break;
            }

            if (pid > 0)
                arrpid[j] = pid;
        }

        // 2, 3, 4, 5, 6, 7 -> 8
        if (i != 1)
        {
            arrpid[i] = getpid();

            pid = fork();
            //childScope
            if (pid == 0)
                i = 8;
                //parentScope
            else if (pid > 0)
                arrpid[8] = pid;
        }

        printf("pid of %d is %d, parent %d\n\n", i, getpid(), getppid());

        //в самом начальном все хранится
        if (i == 1)
        {
            arrpid[i] = getpid();
            filepid = fopen("allpids.txt", "wb+");
            fwrite(arrpid, sizeof(pid_t), 8, filepid);
            fclose(filepid);
        }
        else
            usleep(10000);

        filepid = fopen("allpids.txt", "rb+");
        fread(&arrpid, sizeof(pid_t), 8, filepid);
        fclose(filepid);

        signal(SIGUSR1, signalHandler);
        signal(SIGUSR2, signalHandler);
        signal(SIGTERM, signalHandler);

        usleep(10000);

        switch (i)
        {
            case 1:
                sendSignalToProcess(6, SIGUSR1, SIGNUM);
                break;
            case 6:
                sendSignalToProcess(7, SIGUSR1, SIGNUM);
                break;
            case 7:
                sendSignalToGroup(4, SIGUSR2, SIGNUM);
                break;
            case 4:
                sendSignalToProcess(8, SIGUSR1, SIGNUM);
                break;
            case 5:
                sendSignalToProcess(2, SIGUSR1, SIGNUM);
                break;
            case 8:
                sendSignalToProcess(2, SIGUSR2, SIGNUM);
                break;
            case 2:
                sendSignalToProcess(1, SIGUSR2, SIGNUM);
                break;
        }

        while (true)
            if (flag)
                break;

        waitForChildProcesses();
    }
    else
    {
        printf("pid of 0 process is %d, parent %d\n\n", getpid(), getppid());
        wait(NULL);
    }
}

int main(int argc, char *argv[])
{
    if (argc < 2)
    {
        printf("use <%s> <amount_of_ticks>\n", argv[0]);
        return 0;
    }
    const int SIGCOUNT = atoi(argv[1]);
    FILE *filepid;

    createChildProcesses(filepid, SIGCOUNT);

    return 0;
}
