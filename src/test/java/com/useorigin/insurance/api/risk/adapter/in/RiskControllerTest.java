package com.useorigin.insurance.api.risk.adapter.in;

import com.google.gson.FieldNamingPolicy;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
import com.useorigin.insurance.api.risk.application.service.RiskProfileService;
import com.useorigin.insurance.api.risk.domain.MaritalStatus;
import com.useorigin.insurance.api.risk.domain.ScoreType;
import com.useorigin.insurance.api.risk.domain.service.RiskScoreService;
import com.useorigin.insurance.api.risk.infrastructure.web.out.RiskProfileResource;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.Mockito;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.WebMvcTest;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.http.MediaType;
import org.springframework.test.context.junit.jupiter.SpringExtension;
import org.springframework.test.web.servlet.MockMvc;

import static org.hamcrest.Matchers.is;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.post;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

@WebMvcTest
@ExtendWith(SpringExtension.class)
public class RiskControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @MockBean
    private RiskScoreService riskScoreService;

    @MockBean
    private RiskProfileService riskProfileService;


    private RiskProfileCreationCommand command;

    @BeforeEach
    void setup() {

        Integer[] risks = {1, 1, 1};

        command = new RiskProfileCreationCommand.Builder()
                .atAge(100)
                .withDependents(10)
                .withIncome(100000)
                .withMaritalStatus(MaritalStatus.MARRIED.name())
                .withRisks(risks)
                .build();

    }

    @Test
    void testIneligibleProfile() throws Exception {

        RiskProfileResource mockProfileIneligible = createMockProfileIneligible();
        Mockito.when(riskProfileService.createProfile(Mockito.any())).thenReturn(mockProfileIneligible);

        mockMvc.perform(
                post("/insurances/risk/profile")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(jsonfy(command)))
                .andExpect(status().isCreated())
                .andExpect(jsonPath("$.auto", is("INELIGIBLE")))
                .andExpect(jsonPath("$.disability", is("INELIGIBLE")))
                .andExpect(jsonPath("$.home", is("INELIGIBLE")))
                .andExpect(jsonPath("$.life", is("INELIGIBLE")));
    }

    @Test
    void shouldReturnBadRequestNoRequiredFields() throws Exception {
        RiskProfileCreationCommand payload = createPayloadWithoutRequiredFields();

        mockMvc.perform(
                post("/insurances/risk/profile")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(jsonfy(payload)))
                .andExpect(status().isBadRequest());
    }

    private RiskProfileCreationCommand createPayloadWithoutRequiredFields() {
        return new RiskProfileCreationCommand.Builder().build();
    }

    private String jsonfy(RiskProfileCreationCommand command) {
        Gson gson = new GsonBuilder()
                .setFieldNamingPolicy(FieldNamingPolicy.LOWER_CASE_WITH_UNDERSCORES)
                .create();

        return gson.toJson(command);
    }

    private RiskProfileResource createMockProfileIneligible() {
        return new RiskProfileResource(ScoreType.INELIGIBLE, ScoreType.INELIGIBLE, ScoreType.INELIGIBLE, ScoreType.INELIGIBLE);
    }

}
