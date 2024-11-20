package edu.epam.fop.http.client;

import static org.junit.jupiter.api.Assertions.assertEquals;

import java.net.HttpURLConnection;
import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;

import org.gaul.httpbin.HttpBin;
import org.json.JSONObject;
import org.junit.jupiter.api.AfterAll;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.DisplayNameGeneration;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;

@DisplayNameGeneration(ReplaceCamelCase.class)
class TaskManagerTest {

	private static HttpClient client;

	private static String baseUrl;
	private static URI httpBinEndpoint;
	private static HttpBin httpBin;

	@BeforeAll
	static void init() throws Exception {
		httpBinEndpoint = URI.create("http://127.0.0.1:0");
		client = HttpClient.newHttpClient();
		httpBin = new HttpBin(httpBinEndpoint);
		httpBin.start();
		httpBinEndpoint = new URI(httpBinEndpoint.getScheme(), httpBinEndpoint.getUserInfo(), httpBinEndpoint.getHost(),
				httpBin.getPort(), httpBinEndpoint.getPath(), httpBinEndpoint.getQuery(),
				httpBinEndpoint.getFragment());
		baseUrl = httpBinEndpoint + "/anything";

		URI uri = URI.create(httpBinEndpoint + "/status/200");
		HttpURLConnection conn = (HttpURLConnection) uri.toURL().openConnection();
		assertEquals(200, conn.getResponseCode(), "The status code from the HTTP response message is incorrect");
	}

	@AfterAll
	static void shutdown() throws Exception {
		httpBin.stop();
	}

	@ParameterizedTest
	@CsvSource({ "'1', 'New Task1', 'Started'", "'2', 'New Task2', 'New'", "'abc', 'defg', 'hijkl'" })
	void testCreateTask(String taskId, String taskName, String taskStatus) throws Exception {
		client = HttpClient.newHttpClient();

		HttpRequest request = TaskManager.createTask(baseUrl, taskId, taskName, taskStatus);
		HttpResponse<String> response = client.send(request, HttpResponse.BodyHandlers.ofString());

		int actualStatusCode = response.statusCode();
		assertEquals(200, actualStatusCode, "The status code: " + actualStatusCode);

		String body = response.body();
		JSONObject jsonBody = new JSONObject(body);
		assertEquals("POST", jsonBody.getString("method"), "The HTTP request method is incorrect");
		assertEquals(baseUrl, jsonBody.getString("url"), "The URL is incorrect");

		JSONObject jsonHeaders = jsonBody.getJSONObject("headers");
		assertEquals("application/json", jsonHeaders.getString("Content-Type"), "The Content-Type header is incorrect");

		JSONObject jsonData = new JSONObject(jsonBody.getString("data"));
		assertEquals(taskId, jsonData.getString("taskId"), "The task ID is incorrect");
		assertEquals(taskName, jsonData.getString("taskName"), "The task name is incorrect");
		assertEquals(taskStatus, jsonData.getString("taskStatus"), "The task staus is incorrect");
	}

	@ParameterizedTest
	@CsvSource({ "'1'", "'2'", "'3'", "'123'", "'abc'" })
	void testRetrieveTask(String taskId) throws Exception {
		client = HttpClient.newHttpClient();

		HttpRequest request = TaskManager.retrieveTask(baseUrl, taskId);
		HttpResponse<String> response = client.send(request, HttpResponse.BodyHandlers.ofString());

		int actualStatusCode = response.statusCode();
		assertEquals(200, actualStatusCode, "The status code: " + actualStatusCode);

		String body = response.body();
		JSONObject jsonBody = new JSONObject(body);
		assertEquals("GET", jsonBody.getString("method"), "The HTTP request method is incorrect");
		assertEquals(baseUrl + "/" + taskId, jsonBody.getString("url"), "The URL is incorrect");
	}

	@ParameterizedTest
	@CsvSource({ "'9', 'Update Task9', 'Started'", "'8', 'Update Task8', 'New'", "'cba', 'gfed', 'lkjih'" })
	void testUpdateTask(String taskId, String taskName, String taskStatus) throws Exception {
		client = HttpClient.newHttpClient();

		HttpRequest request = TaskManager.updateTask(baseUrl, taskId, taskName, taskStatus);
		HttpResponse<String> response = client.send(request, HttpResponse.BodyHandlers.ofString());

		int actualStatusCode = response.statusCode();
		assertEquals(200, actualStatusCode, "The status code: " + actualStatusCode);

		String body = response.body();
		JSONObject jsonBody = new JSONObject(body);
		assertEquals("PUT", jsonBody.getString("method"), "The HTTP request method is incorrect");
		assertEquals(baseUrl + "/" + taskId, jsonBody.getString("url"), "The URL is incorrect");

		JSONObject jsonHeaders = jsonBody.getJSONObject("headers");
		assertEquals("application/json", jsonHeaders.getString("Content-Type"), "The Content-Type header is incorrect");

		JSONObject jsonData = new JSONObject(jsonBody.getString("data"));
		assertEquals(taskId, jsonData.getString("taskId"), "The task ID is incorrect");
		assertEquals(taskName, jsonData.getString("taskName"), "The task name is incorrect");
		assertEquals(taskStatus, jsonData.getString("taskStatus"), "The task staus is incorrect");
	}

	@ParameterizedTest
	@CsvSource({ "'9'", "'8'", "'7'", "'321'", "'cba'" })
	void testDeleteTask(String taskId) throws Exception {
		client = HttpClient.newHttpClient();

		HttpRequest request = TaskManager.deleteTask(baseUrl, taskId);
		HttpResponse<String> response = client.send(request, HttpResponse.BodyHandlers.ofString());

		int actualStatusCode = response.statusCode();
		assertEquals(200, actualStatusCode, "The status code: " + actualStatusCode);

		String body = response.body();
		JSONObject jsonBody = new JSONObject(body);
		assertEquals("DELETE", jsonBody.getString("method"), "The HTTP request method is incorrect");
		assertEquals(baseUrl + "/" + taskId, jsonBody.getString("url"), "The URL is incorrect");
	}
}