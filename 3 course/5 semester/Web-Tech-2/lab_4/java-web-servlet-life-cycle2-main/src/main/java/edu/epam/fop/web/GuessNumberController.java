package edu.epam.fop.web;

import java.io.IOException;
import java.util.concurrent.ThreadLocalRandom;

import jakarta.servlet.ServletContext;
import jakarta.servlet.ServletException;
import jakarta.servlet.annotation.WebServlet;
import jakarta.servlet.http.HttpServlet;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;

@WebServlet("/GuessNumber")
public class GuessNumberController extends HttpServlet {
	private static final long serialVersionUID = -1963426021633719325L;

	private static final int MAX_NUMBER_OF_ATTEMPTS = 10;

	private static final String HOME_PAGE = "/index.html";
	private static final String GAME_START_PAGE = "/WEB-INF/start.jsp";
	private static final String GAME_ATTEMPT_PAGE = "/WEB-INF/tryIt.jsp";
	private static final String GAME_END_PAGE = "/WEB-INF/finish.jsp";

	private static final String START_COMMAND = "start";
	private static final String TRY_IT_COMMAND = "tryIt";

	private static final String MIN_NUMBER_PARAMETER = "minNumber";
	private static final String MAX_NUMBER_PARAMETER = "maxNumber";
	private static final String COMMAND_PARAMETER = "command";
	private static final String NUMBER_PARAMETER = "number";
	private static final String USER_NAME_PARAMETER = "name";

	private static final String MIN_NUMBER_ATTRIBUTE = "minNumber";
	private static final String MAX_NUMBER_ATTRIBUTE = "maxNumber";
	private static final String USER_NAME_ATTRIBUTE = "name";
	private static final String RESULT_ATTRIBUTE = "result";
	private static final String TRY_COUNT_ATTRIBUTE = "tryCount";
	private static final String MAX_NUMBER_OF_ATTEMPTS_ATTRIBUTE = "maxAttempts";

	private static final String MISSING_MESSAGE = "Please enter a number";
	private static final String LESS_MESSAGE = "Your number is less than the number being guessed";
	private static final String WIN_MESSAGE = "You win!";
	private static final String GREATER_MESSAGE = "Your number is greater than the number being guessed";
	private static final String INVALID_MESSAGE = "Invalid value entered";
	private static final String LOSE_MESSAGE = "You lose";

	enum Status {
		MISSING, LESS, WIN, GREATER, INVALID, LOSE
	}

	// Use this object for logging.
	System.Logger logger = System.getLogger(GuessNumberController.class.getName());

	// Use this field to store user name.
	String name;

	// Use this field to store the lower limit of a random number range.
	int minNumber;

	// Use this field to store the upper limit of a random number range.
	int maxNumber;

	// Use this field to store a random (secret) number.
	int randomNumber;

	// Use this field as the counter of attempts.
	int tryCount;

	@Override
	public void init() throws ServletException {
		ServletContext context = getServletContext();

		String minNumberParam = getInitParameter(MIN_NUMBER_PARAMETER);
		String maxNumberParam = getInitParameter(MAX_NUMBER_PARAMETER);

		context.setAttribute(MAX_NUMBER_OF_ATTEMPTS_ATTRIBUTE, MAX_NUMBER_OF_ATTEMPTS);

		try {
			minNumber = (minNumberParam != null) ? Integer.parseInt(minNumberParam) : 1;
		} catch (NumberFormatException e) {
			minNumber = 1;
			logger.log(System.Logger.Level.WARNING, "Invalid minNumber parameter, defaulting to 1.");
		}

		try {
			maxNumber = (maxNumberParam != null) ? Integer.parseInt(maxNumberParam) : 50;
		} catch (NumberFormatException e) {
			maxNumber = 50;
			logger.log(System.Logger.Level.WARNING, "Invalid maxNumber parameter, defaulting to 50.");
		}

		// Store these values in the ServletContext as attributes
		context.setAttribute(MIN_NUMBER_ATTRIBUTE, minNumber);
		context.setAttribute(MAX_NUMBER_ATTRIBUTE, maxNumber);

		logger.log(System.Logger.Level.DEBUG, "Servlet initialized with minNumber=" + minNumber + ", maxNumber=" + maxNumber);
	}

	@Override
	public void destroy() {
		logger.log(System.Logger.Level.DEBUG, "Servlet complete");
	}

	@Override
	protected void service(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {

		String command = request.getParameter(COMMAND_PARAMETER);
		if (command == null) {
			command = "";
		}

		String pathToForward;

		switch (command) {
			case START_COMMAND:
				pathToForward = handleStart(request);
				break;
			case TRY_IT_COMMAND:
				pathToForward = handleTry(request);
				break;
			default:
				pathToForward = handleDefault();
				break;
		}

		request.getRequestDispatcher(pathToForward).forward(request, response);

		logger.log(System.Logger.Level.INFO,
				"minNumber: {0}, maxNumber: {1}, randomNumber: {2}, name: {3}, tryCount: {4}, command: {5}, page: {6}",
				minNumber, maxNumber, randomNumber, name, tryCount, command, pathToForward);
	}


	String handleDefault() {
		return HOME_PAGE;
	}

	String handleStart(HttpServletRequest request) {
		tryCount = 0;

		name = request.getParameter(USER_NAME_PARAMETER);

		if (name == null || name.isEmpty()) {
			name = "Player";
		}

		request.setAttribute(USER_NAME_ATTRIBUTE, name);
		randomNumber = ThreadLocalRandom.current().nextInt(minNumber, maxNumber + 1);

		logger.log(System.Logger.Level.DEBUG, "Game started for " + name + " with randomNumber=" + randomNumber);
		return GAME_START_PAGE;
	}

	String handleTry(HttpServletRequest request) {
		String page = GAME_ATTEMPT_PAGE;
		String enteredString = request.getParameter(NUMBER_PARAMETER);
		Status status = handleEnteredNumber(enteredString);

		switch (status) {
		case MISSING:
			request.setAttribute(RESULT_ATTRIBUTE, MISSING_MESSAGE);
			break;
		case INVALID:
			request.setAttribute(RESULT_ATTRIBUTE, INVALID_MESSAGE);
			break;
		case LESS:
			request.setAttribute(RESULT_ATTRIBUTE, LESS_MESSAGE);
			break;
		case GREATER:
			request.setAttribute(RESULT_ATTRIBUTE, GREATER_MESSAGE);
			break;
		case WIN:
			request.setAttribute(RESULT_ATTRIBUTE, WIN_MESSAGE);
			page = GAME_END_PAGE;
			break;
		case LOSE:
			request.setAttribute(RESULT_ATTRIBUTE, LOSE_MESSAGE);
			page = GAME_END_PAGE;
			break;
		}
		tryCount++;
		request.setAttribute(TRY_COUNT_ATTRIBUTE, tryCount);
		return page;
	}

	private Status handleEnteredNumber(String enteredString) {
		Status status;
		if (enteredString == null) {
			status = Status.MISSING;
		} else {
			try {
				int enteredNumber = Integer.parseInt(enteredString);
				status = (enteredNumber < randomNumber) ? Status.LESS
						: ((enteredNumber == randomNumber) ? Status.WIN : Status.GREATER);
			} catch (NumberFormatException e) {
				status = Status.INVALID;
			}
		}
		if ((tryCount > MAX_NUMBER_OF_ATTEMPTS) || (tryCount == MAX_NUMBER_OF_ATTEMPTS && status != Status.WIN)) {
			status = Status.LOSE;
		}
		return status;
	}
}