<%@ page language="java" contentType="text/html; charset=UTF-8"
	pageEncoding="UTF-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core"%>
<!DOCTYPE html>
<html lang="en">
<head>
<title>Search Book</title>
</head>
<body>
	<c:choose>
		<c:when test="${not empty sessionScope.user}"><p>Hello <c:out value="${sessionScope.user}" />!</p></c:when>
		<c:otherwise><p><a href="login.jsp">Login</a></p></c:otherwise>
	</c:choose>

	<form action="searchBook" method="get">
		<input type="text" name="isbn" placeholder="ISBN"> <input type="submit" value="Search">
	</form>
	<br>
	<form action="viewFines" method="get">
		<input type="submit" value="View fines">
	</form>
</body>
</html>