<%@ page language="java" contentType="text/html; charset=UTF-8"
	pageEncoding="UTF-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core"%>
<!DOCTYPE html>
<html lang="en">
<head>
<title>Result</title>
</head>
<body>
	<c:if test="${not empty sessionScope.error}">
		<h2>Error</h2>
		<p style="color: red">
			<c:out value="${sessionScope.error}" />
		</p>
	</c:if>
	<h2>History</h2>
	<ul>
		<c:forEach var="historyItem" items="${sessionScope.history}">
			<li><c:out value="${historyItem}" /></li>
		</c:forEach>
	</ul>

	<form action="CalculatorServlet" method="post">
		<input type="hidden" name="action" value="clearHistory"> <input
			type="submit" value="Clear History">
	</form>

	<p>
		<a href="index.html">Go Back</a>
	</p>
</body>
</html>