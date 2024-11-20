<%@ page language="java" contentType="text/html; charset=UTF-8"
	pageEncoding="UTF-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core"%>
<!DOCTYPE html>
<html lang="en">

<head>
<title>Welcome</title>
</head>

<body>
	<h2>User:</h2>
	<c:choose>
		<c:when test="${not empty sessionScope.role}">
			<p>
				<c:out value="${sessionScope.role}" />
			</p>
		</c:when>
		<c:otherwise>
			<p style="color: red">Anonymous</p>
		</c:otherwise>
	</c:choose>
	<h2>Actions:</h2>
	<form action="commandHandler" method="post">
		<label for="submitLogin">1) </label><input type="text" name="role" placeholder="User role: student, teacher">
		<input type="hidden" name="command" value="login"><input
			type="submit" id="submitLogin" value="Login">
	</form>
	<br>
	<form action="commandHandler" method="post">
		<label for="submitLogout">2) </label><input type="hidden"
			name="command" value="logout"><input type="submit"
			id="submitLogout" value="Logout">
	</form>
	<br>
	<form action="commandHandler" method="post">
		<label for="submitViewSettings">3) </label><input type="hidden"
			name="command" value="viewSettings"><input type="submit"
			id="submitViewSettings" value="View Settings">
	</form>
	<br>
	<form action="commandHandler" method="post">
		<label for="submitUpdateSettings">4) </label><input type="hidden"
			name="command" value="updateSettings"><input type="submit"
			id="submitUpdateSettings" value="Update Settings">
	</form>
</body>

</html>