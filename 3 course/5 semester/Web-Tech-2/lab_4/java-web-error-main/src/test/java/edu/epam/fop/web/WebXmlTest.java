package edu.epam.fop.web;

import static org.junit.jupiter.api.Assertions.assertEquals;

import java.io.File;
import java.util.HashMap;
import java.util.Map;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;

import org.junit.jupiter.api.DisplayNameGeneration;
import org.junit.jupiter.api.Test;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

@DisplayNameGeneration(ReplaceCamelCase.class)
public class WebXmlTest {

	@Test
	void testWebXml() throws Exception {
		File file = new File("src/main/webapp/WEB-INF/web.xml");
		DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
		DocumentBuilder db = dbf.newDocumentBuilder();
		Document doc = db.parse(file);
		doc.getDocumentElement().normalize();

		NodeList nodeList = doc.getElementsByTagName("error-page");
		Map<String, String> errorPages = new HashMap<>();
		for (int itr = 0; itr < nodeList.getLength(); itr++) {
			Node node = nodeList.item(itr);
			if (node.getNodeType() == Node.ELEMENT_NODE) {
				Element eElement = (Element) node;

				if (eElement.getElementsByTagName("exception-type").getLength() > 0) {
					errorPages.put(eElement.getElementsByTagName("exception-type").item(0).getTextContent(),
							eElement.getElementsByTagName("location").item(0).getTextContent());
				} else {
					errorPages.put(eElement.getElementsByTagName("error-code").item(0).getTextContent(),
							eElement.getElementsByTagName("location").item(0).getTextContent());
				}
			}
		}

		assertEquals("/error.jsp", errorPages.get("edu.epam.fop.web.exceptions.InvalidISBNException"),
				"Any servlet execution that throws the InvalidISBNException should redirect the user to the error.jsp page");
		assertEquals("/error.jsp", errorPages.get("404"),
				"For HTTP 404 error, the user should be redirected to the error.jsp page");
		assertEquals("/login.jsp", errorPages.get("403"),
				"For HTTP 403 error, the user should be redirected to the login.jsp page");
	}
}