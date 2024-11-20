<%@ page isErrorPage="true" language="java"
	contentType="text/html; charset=UTF-8" pageEncoding="UTF-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core"%>
<!DOCTYPE html>
<html lang="en">
<head>
<title>Error Page</title>
</head>
<body>
	<h2>An error has occurred</h2>
	<p>
		Status code: <c:out value="${pageContext.errorData.statusCode}" />
	</p>
	<p>
		<c:out value="${pageContext.exception.message}" />
	</p>
	<p>
		<a href="home.jsp">Go back to home page</a>
	</p>
</body>
</html>