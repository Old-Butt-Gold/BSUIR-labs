<%@ page language="java" contentType="text/html; charset=UTF-8"
	pageEncoding="UTF-8"%>
<!DOCTYPE html>
<html lang="en">
<head>
<title>Calculator Result</title>
</head>
<body>
	<h2>Result</h2>
	${calculation.result}
	<hr>
	<br> Operation: ${calculation.operation}
	<br> num1: ${calculation.num1}
	<br> num2: ${calculation.num2}
</body>
</html>