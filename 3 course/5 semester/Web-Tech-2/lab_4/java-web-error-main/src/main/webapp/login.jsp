<%@ page language="java"
	contentType="text/html; charset=UTF-8" pageEncoding="UTF-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core"%>
<!DOCTYPE html>
<html lang="en">
<head>
<title>Login</title>
</head>
<body>
	<h2>Login Form</h2>
	<p style="color: red;">
		<c:if test="${not empty requestScope.errorMessage}">
			<c:out value="${errorMessage}" />
		</c:if>
		&nbsp;
	</p>
	<form action="login" method="post">
		Username: <input type="text" name="username"><br>
		<br> Password: <input type="password" name="password"><br>
		<br> <input type="submit" value="Login">
	</form>
	<p>
		<br><a href="home.jsp">Go back to home page</a>
	</p>
</body>
</html>