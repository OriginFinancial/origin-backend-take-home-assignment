package com.useorigin.insurance.api.risk.application.service;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
import com.useorigin.insurance.api.risk.domain.*;
import com.useorigin.insurance.api.risk.domain.service.RiskScoreService;
import com.useorigin.insurance.api.risk.infrastructure.web.out.RiskProfileResource;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.test.util.ReflectionTestUtils;

import static org.junit.jupiter.api.Assertions.assertEquals;

class ProfileServiceTest {

    private RiskScoreService riskScoreService;
    private RiskProfileService riskProfileService;
    private RiskProfileCreationCommand command;

    @BeforeEach
    void setup() {
        riskScoreService = new RiskScoreService();
        riskProfileService = new RiskProfileService(riskScoreService);

        Integer[] risks = {1, 1, 1};
        Vehicle vehicle = new Vehicle(2019);
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
    void shouldReturnIneligibleForDisabilityWhenHasNoIncome() {
        ReflectionTestUtils.setField(command, "income", 0);

        RiskProfileResource profile = riskProfileService.createProfile(command);

        assertEquals(ScoreType.INELIGIBLE, profile.getDisability());
    }

    @Test
    void shouldReturnIneligibleForAutoWhenHasNoVehicle() {
        ReflectionTestUtils.setField(command, "vehicle", null);

        RiskProfileResource profile = riskProfileService.createProfile(command);

        assertEquals(ScoreType.INELIGIBLE, profile.getAuto());
    }

    @Test
    void shouldReturnIneligibleForHomeWhenHasNoHouse() {
        ReflectionTestUtils.setField(command, "house", null);

        RiskProfileResource profile = riskProfileService.createProfile(command);

        assertEquals(ScoreType.INELIGIBLE, profile.getHome());
    }

    @Test
    void shouldReturnIneligibleForDisabilityAndLifeWhenAgeIsOver60() {
        ReflectionTestUtils.setField(command, "age", 61);

        RiskProfileResource profile = riskProfileService.createProfile(command);

        assertEquals(ScoreType.INELIGIBLE, profile.getDisability());
        assertEquals(ScoreType.INELIGIBLE, profile.getLife());
    }

    @Test
    void testInputExample() {
        Integer[] risks = {0, 1, 0};
        Vehicle vehicle = new Vehicle(2018);
        House house = new House(OwnershipStatus.OWNED);

        ReflectionTestUtils.setField(command, "age", 35);
        ReflectionTestUtils.setField(command, "dependents", 2);
        ReflectionTestUtils.setField(command, "house", house);
        ReflectionTestUtils.setField(command, "income", 0);
        ReflectionTestUtils.setField(command, "maritalStatus", MaritalStatus.MARRIED);
        ReflectionTestUtils.setField(command, "riskQuestions", risks);
        ReflectionTestUtils.setField(command, "vehicle", vehicle);


        RiskProfileResource profile = riskProfileService.createProfile(command);

        assertEquals(ScoreType.REGULAR, profile.getAuto());
        assertEquals(ScoreType.INELIGIBLE, profile.getDisability());
        assertEquals(ScoreType.ECONOMIC, profile.getHome());
        assertEquals(ScoreType.REGULAR, profile.getLife());
    }
}