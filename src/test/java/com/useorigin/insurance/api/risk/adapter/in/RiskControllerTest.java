package com.useorigin.insurance.api.risk.adapter.in;

import com.google.gson.FieldNamingPolicy;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
import com.useorigin.insurance.api.risk.domain.House;
import com.useorigin.insurance.api.risk.domain.OwnershipStatus;
import com.useorigin.insurance.api.risk.domain.command.MaritalStatus;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.WebMvcTest;
import org.springframework.http.MediaType;
import org.springframework.test.context.junit.jupiter.SpringExtension;
import org.springframework.test.web.servlet.MockMvc;

import static org.hamcrest.Matchers.is;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.post;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

@ExtendWith(SpringExtension.class)
@WebMvcTest(controllers = RiskController.class)
public class RiskControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @Test
    void testNothing() throws Exception {

        RiskProfileCreationCommand payload = createPayload();

        mockMvc.perform(
                post("/insurances/risk")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(jsonfy(payload)))
                .andExpect(status().isCreated())
                .andExpect(jsonPath("$.auto", is("regular")))
                .andExpect(jsonPath("$.disability", is("ineligible")))
                .andExpect(jsonPath("$.home", is("economic")))
                .andExpect(jsonPath("$.life", is("regular")));
    }

    @Test
    void shouldReturnBadRequestNoRequiredFields() throws Exception {
        RiskProfileCreationCommand payload = createPayloadWithoutRequiredFields();

        mockMvc.perform(
                post("/insurances/risk")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(jsonfy(payload)))
                .andExpect(status().isBadRequest());
    }

    private RiskProfileCreationCommand createPayloadWithoutRequiredFields() {
        return new RiskProfileCreationCommand.Builder().build();
    }

    private RiskProfileCreationCommand createPayload() {

        House house = new House(OwnershipStatus.OWNED);

        Integer[] risks = {0, 1, 0};

        return new RiskProfileCreationCommand.Builder()
                .atAge(35)
                .withDependents(2)
                .withHouse(house)
                .withIncome(0)
                .withMaritalStatus(MaritalStatus.MARRIED)
                .withRisks(risks)
                .build();
    }

    private String jsonfy(RiskProfileCreationCommand command) {
        Gson gson = new GsonBuilder()
                .setFieldNamingPolicy(FieldNamingPolicy.LOWER_CASE_WITH_UNDERSCORES)
                .create();

        return gson.toJson(command);
    }

}
