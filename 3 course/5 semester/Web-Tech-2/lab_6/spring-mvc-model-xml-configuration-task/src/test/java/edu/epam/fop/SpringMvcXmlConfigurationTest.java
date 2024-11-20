package edu.epam.fop;

import edu.epam.fop.model.FormData;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.test.context.junit.jupiter.web.SpringJUnitWebConfig;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders;
import org.springframework.test.web.servlet.setup.MockMvcBuilders;
import org.springframework.web.context.WebApplicationContext;

import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.model;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.view;

@SpringJUnitWebConfig(locations = "classpath:spring-config.xml")
public class SpringMvcXmlConfigurationTest {

    private MockMvc mockMvc;

    @BeforeEach
    public void setup(WebApplicationContext wac) {
        this.mockMvc = MockMvcBuilders.webAppContextSetup(wac).build();
    }

    @Test
    public void shouldReachGetFormEndpoint() throws Exception {
        mockMvc.perform(MockMvcRequestBuilders.get("/form")).andExpectAll(status().isOk(),
                view().name("formTemplate"));
    }

    @Test
    public void shouldReachPostFormEndpoint() throws Exception {
        mockMvc.perform(MockMvcRequestBuilders.post("/processForm")
                        .queryParam("name", "someName")
                        .queryParam("age", String.valueOf(123)))
                .andExpectAll(status().isOk(), view().name("resultTemplate"),
                        model().attribute("formData", new FormData("someName", 123)));
    }

}
