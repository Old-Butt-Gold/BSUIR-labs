<%@ page language="java" contentType="text/html; charset=UTF-8"
	pageEncoding="UTF-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core"%>
<!DOCTYPE html>
<html lang="en">
<head>
<title>Result</title>
</head>
<body>
	<p>
		<c:choose>
			<c:when test="${not empty requestScope.role}">The <b><c:out
						value="${requestScope.command}" /></b> command is requested by the user <b><c:out
						value="${requestScope.role}" /></b>.</c:when>
			<c:otherwise>The <b><c:out
						value="${requestScope.command}" /></b> command is requested by an unregistered user.</c:otherwise>
		</c:choose>
	</p>
	<p>
		<a href="index.jsp">Go Back</a>
	</p>
</body>
</html>