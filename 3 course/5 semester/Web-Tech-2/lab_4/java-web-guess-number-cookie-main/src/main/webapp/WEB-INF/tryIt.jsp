<%@ page language="java" contentType="text/html; charset=UTF-8"
	pageEncoding="UTF-8"%>
<!DOCTYPE html>
<html lang="en">
<head>
<title>Guess the Number with Cookie</title>
</head>
<body>
	<h2>${result}</h2>
	<p>
		Attempt <b>#${tryCount}</b>
	</p>
	<form action="GuessNumberCookieController">
		<input type="hidden" name="command" value="tryIt" /> Enter your
		number <input type="number" name="number" /> <input type="submit" />
	</form>
	<p>
		<a href="index.html">Let's play from the beginning?</a>
	</p>
</body>
</html>
