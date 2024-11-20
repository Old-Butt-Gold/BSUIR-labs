package edu.epam.fop.presentation.web;

import java.nio.charset.StandardCharsets;
import java.util.EnumSet;
import org.springframework.web.WebApplicationInitializer;
import org.springframework.web.context.support.AnnotationConfigWebApplicationContext;
import org.springframework.web.filter.CharacterEncodingFilter;
import org.springframework.web.servlet.DispatcherServlet;
import jakarta.servlet.DispatcherType;
import jakarta.servlet.ServletContext;

class MyAppInitializer implements WebApplicationInitializer {

  @Override
  public void onStartup(ServletContext container) {
    var webAppContext = new AnnotationConfigWebApplicationContext();
    webAppContext.register(WebConfig.class);
    webAppContext.setServletContext(container);
    webAppContext.refresh();
    
    var springDispatcherServlet =
        container.addServlet("springDispatcherServlet", new DispatcherServlet(webAppContext));
    springDispatcherServlet.setLoadOnStartup(1);
    springDispatcherServlet.addMapping("/");
    springDispatcherServlet.setInitParameter("spring.profiles.active", "dev");
    springDispatcherServlet.setInitParameter("spring.profiles.default", "dev");
    springDispatcherServlet.setInitParameter("spring.liveBeansView.mbeanDomain", "dev");
  
    var charsetFilter = new CharacterEncodingFilter();
    charsetFilter.setEncoding(StandardCharsets.UTF_8.name());
    charsetFilter.setForceEncoding(false);
    var filterRegistration = container.addFilter("charsetEncodingFilter", charsetFilter);
    filterRegistration.addMappingForUrlPatterns(
        EnumSet.of(DispatcherType.REQUEST, DispatcherType.ERROR, DispatcherType.FORWARD), true,
        "/*");

  }

}

