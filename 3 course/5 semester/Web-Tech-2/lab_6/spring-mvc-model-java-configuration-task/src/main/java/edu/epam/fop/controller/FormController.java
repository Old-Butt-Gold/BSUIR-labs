package edu.epam.fop.controller;

import edu.epam.fop.model.FormData;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.servlet.ModelAndView;

/** Add controller implementation here. */
@Controller
public class FormController {
    @GetMapping("/form")
    public ModelAndView getForm(){
        ModelAndView mav = new ModelAndView("formTemplate");
        mav.addObject("formData", new FormData());
        return mav;
    }

    @PostMapping("/processForm")
    public ModelAndView postData(@RequestParam(name = "name") String name, @RequestParam(name = "age")String age){
        ModelAndView mav = new ModelAndView("resultTemplate");
        mav.addObject("formData", new FormData(name, Integer.parseInt(age)));
        return mav;
    }
}