<%@ page language="java" contentType="text/html; charset=UTF-8"
	pageEncoding="UTF-8"%>
<!DOCTYPE html>
<html lang="en">
<head>
<title>Guess the Number with Cookie</title>
</head>
<body>
	<h2>Hello ${name}!</h2>
	<p>
		I guessed a number from <b>${minNumber}</b> to <b>${maxNumber}</b>.<br> Try to
		guess it in <b>${maxAttempts}</b> tries.
	</p>
	<form action="GuessNumberCookieController">
		<input type="hidden" name="command" value="tryIt" /> <input
			type="submit" value="Start!" />
	</form>
	<p>
		<a href="index.html">Let's play from the beginning?</a>
	</p>
</body>
</html>
