<%@ page language="java" contentType="text/html; charset=UTF-8"
	pageEncoding="UTF-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core"%>
<!DOCTYPE html>
<html lang="en">
<head>
<title>Error</title>
</head>
<body>
	<c:if test="${not empty sessionScope.error}">
		<h2>Error</h2>
		<p style="color: red">
			<c:out value="${sessionScope.error}" />
		</p>
	</c:if>

	<p>
		<a href="index.html">Go Back</a>
	</p>
</body>
</html>