#include "stdio.h"
#include "math.h"

int main() {
    int number;
    scanf("%d", &number);
    printf("Task10: number = %d, 2^n = %d \n", number, (int)pow(2, number));

    double a;
    int n;
    scanf("%lf %d", &a, &n);
    printf("Task11: a) result = %.2f \n", pow(a, n));

    double result = 1;
    for (int i = 0; i < n; i++)
    {
        result *= a + i;
    }
    printf("Task11: b) result = %.5f \n", result);


    double resul = 1.0L;
    for (double i = 1; i <= 100; i++) {
        resul *= 1.0L + sin((double)i / 10.0L);
    }
    printf("Task13: result = %e\n", resul);

    scanf("%d %d", &a, &n);
    double res = 1.0 / a;
    for (int temp = 2; temp <= 2 * n; temp += 2) {
        res += 1.0L / pow(a, temp);
    }
    printf("Task12: result = %.20f\n", res);

    double resFinal = 0.0;
    for (int i = 1; i <= 100; i++) {
        resFinal += 1.0 / pow(i, 2);
    }
    printf("Task16: result = %.20f", resFinal);
    return 0;
}
