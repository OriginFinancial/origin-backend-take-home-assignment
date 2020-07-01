package com.useorigin.insurance.api.risk.adapter.in;

import com.google.gson.Gson;
import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
import com.useorigin.insurance.api.risk.domain.House;
import com.useorigin.insurance.api.risk.domain.OwnershipStatus;
import com.useorigin.insurance.api.risk.domain.Vehicle;
import com.useorigin.insurance.api.risk.domain.command.MaritalStatus;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.WebMvcTest;
import org.springframework.http.MediaType;
import org.springframework.test.context.junit.jupiter.SpringExtension;
import org.springframework.test.web.servlet.MockMvc;

import java.time.Year;

import static org.hamcrest.Matchers.is;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.post;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

@ExtendWith(SpringExtension.class)
@WebMvcTest(controllers = InsuranceController.class)
public class InsuranceControllerTest {

    @Autowired
    private MockMvc mockMvc;

    private String jsonfy(RiskProfileCreationCommand command) {
        Gson gson = new Gson();
        return gson.toJson(command);
    }

    @Test
    void testNothing() throws Exception {

        RiskProfileCreationCommand payload = createPayload();

        mockMvc.perform(
                post("/insurances/risk", 1L)
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(jsonfy(payload)))
                .andExpect(status().isCreated())
                .andExpect(jsonPath("$.auto", is("regular")))
                .andExpect(jsonPath("$.disability", is("ineligible")))
                .andExpect(jsonPath("$.home", is("economic")))
                .andExpect(jsonPath("$.life", is("regular")));
    }

    private RiskProfileCreationCommand createPayload() {

        House house = new House(OwnershipStatus.OWNED);
        Vehicle vehicle = new Vehicle(Year.of(2019));

        boolean[] risks = {false, true, false};

        return new RiskProfileCreationCommand.Builder()
                .atAge(35)
                .withDependents(2)
                .withHouse(house)
                .withIncome(0)
                .withMaritalStatus(MaritalStatus.MARRIED)
                .withRisks(risks)
                .withCar(vehicle)
                .build();
    }

}
