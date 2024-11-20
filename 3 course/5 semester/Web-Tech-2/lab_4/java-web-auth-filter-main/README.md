# Authorization Filter

The goal of this task is to build an authentication filter for a school management system.

*Duration*: 45 minutes.

## Description

The application must be able to execute specific commands based on the role of a user who is logged in. These commands should be validated by an authorization filter:
   - The "login" command should only be executed for a user who is not logged in.
   - The "logout" command should only be executed for a user who is logged in.
   - Lists of allowed commands for specific users are specified in the deployment descriptor (web.xml) as corresponding initialization parameters.
   - If a command isn't allowed, the filter should respond with an HTTP 403 error indicating forbidden access.

## Requirements

1) Use the `index.jsp` page to send commands for performing various actions that match the role of the user who is logged in.

2) Design a class called `AuthorizationFilter` as a servlet filter to intercept HTTP requests and check if the command given in a request as the `command` parameter is authorized for the user role stored in the HTTP session as the `role` attribute.

3) Implement the `AuthorizationFilter.init` method to initialize the `roleCommands` map, which stores roles as keys and commands allowed for each role as a set of strings. The deployment descriptor for each role specifies a list of allowed commands, separated by a space.

4) Implement the `AuthorizationFilter.doFilter` method to check if the command from the request is allowed for the role stored in the session. If the command is allowed, the request should be forwarded down the filter chain. Otherwise, the HTTP `403 Forbidden` error should be returned by calling the `HttpServletResponse.sendError` method.

## Examples

An example of specifying the list of allowed commands for the users "teacher" and "student" in the deployment descriptor:

```xml
<filter>
	<filter-name>AuthorizationFilter</filter-name>
	<filter-class>edu.epam.fop.web.AuthorizationFilter</filter-class>
	<init-param>
		<param-name>teacher</param-name>
		<param-value>viewSettings updateSettings</param-value>
	</init-param>
	<init-param>
		<param-name>student</param-name>
		<param-value>viewSettings</param-value>
	</init-param>
</filter>
```

The "viewSettings" and "updateSettings" commands are allowed for the user "teacher." The "viewSettings" command is allowed for the user "student."

An example of a partial implementation of authorization filter methods:

```java
@Override
public void init(FilterConfig config) throws ServletException {
	roleCommands = new HashMap<>();
	Enumeration<String> paramNames = config.getInitParameterNames();
	...
}

@Override
public void doFilter(ServletRequest request, ServletResponse response, FilterChain chain)
		throws IOException, ServletException {

	HttpServletRequest httpRequest = (HttpServletRequest) request;
	String command = httpRequest.getParameter(COMMAND_PARAMETER_NAME);
	String role = (String) httpRequest.getSession().getAttribute(USER_ROLE_ATTRIBUTE_NAME);
	Set<String> allowedCommands;
	...
	allowedCommands = roleCommands.get(role);
	...
}
```
