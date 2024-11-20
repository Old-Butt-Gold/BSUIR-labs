<%@ page language="java" contentType="text/html; charset=UTF-8"
	pageEncoding="UTF-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core"%>
<!DOCTYPE html>
<html lang="en">
<head>
<title>Result</title>
</head>
<body>
	<c:choose>
		<c:when test="${not empty requestScope.result}"><p><c:out value="${requestScope.result}" /></p></c:when>
		<c:otherwise><p style="color: red;">The result must be specified as a request attribute.</p></c:otherwise>
	</c:choose>
	<p>
		<a href="home.jsp">Go back to home page</a>
	</p>
</body>
</html>