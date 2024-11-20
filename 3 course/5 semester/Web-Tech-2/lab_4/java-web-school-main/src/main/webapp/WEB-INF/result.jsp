<%@ page language="java" contentType="text/html; charset=UTF-8"
	pageEncoding="UTF-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core"%>
<!DOCTYPE html>
<html lang="en">
<head>
<title>Result</title>
</head>
<body>
	<h2>
		Sessions:
		<c:out value="${applicationScope.activeUsersCounter}" />
	</h2>
	<h2>Students:</h2>
	<ul>
		<c:forEach var="student" items="${requestScope.studentList}">
			<li><c:out value="${student}" /></li>
		</c:forEach>
	</ul>
	<h2>Teachers:</h2>
	<ul>
		<c:forEach var="teacher" items="${requestScope.teacherList}">
			<li><c:out value="${teacher}" /></li>
		</c:forEach>
	</ul>
</body>
</html>