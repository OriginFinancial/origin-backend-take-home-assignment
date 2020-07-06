package com.useorigin.insurance.api.risk.application.service;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
import com.useorigin.insurance.api.risk.domain.*;
import com.useorigin.insurance.api.risk.domain.service.RiskScoreService;
import com.useorigin.insurance.api.risk.infrastructure.web.out.RiskProfileResource;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.test.util.ReflectionTestUtils;

import java.time.Year;

import static org.junit.jupiter.api.Assertions.assertEquals;

class RiskProfileServiceTest {

    private RiskScoreService riskScoreService;
    private RiskProfileService riskProfileService;
    private RiskProfileCreationCommand command;

    @BeforeEach
    void setup() {
        riskScoreService = new RiskScoreService();
        riskProfileService = new RiskProfileService(riskScoreService);

        Integer[] risks = {1, 1, 1};
        Vehicle vehicle = new Vehicle(Year.of(2019));
        House house = new House(OwnershipStatus.OWNED);

        command = new RiskProfileCreationCommand.Builder()
                .atAge(100)
                .withDependents(10)
                .withIncome(100000)
                .withMaritalStatus(MaritalStatus.MARRIED)
                .withRisks(risks)
                .withHouse(house)
                .withVehicle(vehicle)
                .build();
    }

    @Test
    void shouldReturnIneligibleForAllInsuranceWhenHasNoCarNoHouseNoIncome() {
        ReflectionTestUtils.setField(command, "income", 0);
        ReflectionTestUtils.setField(command, "house", null);
        ReflectionTestUtils.setField(command, "vehicle", null);

        RiskProfileResource profile = riskProfileService.createProfile(command);

        assertEquals(ScoreType.INELIGIBLE, profile.getAuto());
        assertEquals(ScoreType.INELIGIBLE, profile.getDisability());
        assertEquals(ScoreType.INELIGIBLE, profile.getHome());
        assertEquals(ScoreType.INELIGIBLE, profile.getLife());
    }
}