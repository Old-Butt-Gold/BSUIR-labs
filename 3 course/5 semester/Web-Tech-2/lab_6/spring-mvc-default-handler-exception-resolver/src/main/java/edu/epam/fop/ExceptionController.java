package edu.epam.fop;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;

@Controller
public class ExceptionController {

    @GetMapping("/triggerException")
    public String triggerException() {
        // This will trigger a NumberFormatException
        String value = "not-a-number";
        Integer.parseInt(value);
        return "error";
    }

    @GetMapping("/nullPointerException")
    public String nullPointerException() {
        // This will trigger a NullPointerException
        String value = null;
        value.length();
        return "error";
    }
}
