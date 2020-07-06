package com.useorigin.insurance.api.risk.domain.service;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
import com.useorigin.insurance.api.risk.domain.*;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.test.util.ReflectionTestUtils;

import java.time.Year;

import static org.junit.jupiter.api.Assertions.assertEquals;

class RiskScoreServiceTest {

    RiskScoreService service = new RiskScoreService();
    private RiskProfileCreationCommand command;

    @BeforeEach
    void setup() {
        Integer[] risks = {1, 1, 1};

        command = new RiskProfileCreationCommand.Builder()
                .atAge(100)
                .withDependents(0)
                .withIncome(100000)
                .withMaritalStatus(MaritalStatus.SINGLE)
                .withRisks(risks)
                .build();
    }

    @Test
    void shouldReturnDefaultRiskBasedOnRiskQuestions() {
        RiskScore riskScore = service.calculate(command);

        assertEquals(3, riskScore.getScoreAuto().getScore());
        assertEquals(3, riskScore.getScoreDisability().getScore());
        assertEquals(3, riskScore.getScoreHome().getScore());
        assertEquals(3, riskScore.getScoreLife().getScore());
    }

    @Test
    void shouldDeductTwoRiskPointsWhenUserIsUnder30() {
        ReflectionTestUtils.setField(command, "age", 20);
        RiskScore riskScore = service.calculate(command);

        assertEquals(1, riskScore.getScoreAuto().getScore());
        assertEquals(1, riskScore.getScoreDisability().getScore());
        assertEquals(1, riskScore.getScoreHome().getScore());
        assertEquals(1, riskScore.getScoreLife().getScore());
    }

    @Test
    void shouldDeductOneRiskPointWhenUserIsBetween30And40() {
        ReflectionTestUtils.setField(command, "age", 39);
        RiskScore riskScore = service.calculate(command);

        assertEquals(2, riskScore.getScoreAuto().getScore());
        assertEquals(2, riskScore.getScoreDisability().getScore());
        assertEquals(2, riskScore.getScoreHome().getScore());
        assertEquals(2, riskScore.getScoreLife().getScore());
    }

    @Test
    void shouldDeductOneRiskPointWhenIncomeIsAbove200K() {
        ReflectionTestUtils.setField(command, "age", 39);
        ReflectionTestUtils.setField(command, "income", 201000);
        RiskScore riskScore = service.calculate(command);

        assertEquals(1, riskScore.getScoreAuto().getScore());
        assertEquals(1, riskScore.getScoreDisability().getScore());
        assertEquals(1, riskScore.getScoreHome().getScore());
        assertEquals(1, riskScore.getScoreLife().getScore());
    }

    @Test
    void shouldAddOneRiskPointToHomeAndDisabilityWhenHouseIsMortgaged() {
        House house = new House(OwnershipStatus.MORTGAGED);
        ReflectionTestUtils.setField(command, "age", 39);
        ReflectionTestUtils.setField(command, "income", 201000);
        ReflectionTestUtils.setField(command, "house", house);

        RiskScore riskScore = service.calculate(command);

        assertEquals(1, riskScore.getScoreAuto().getScore());
        assertEquals(2, riskScore.getScoreDisability().getScore());
        assertEquals(2, riskScore.getScoreHome().getScore());
        assertEquals(1, riskScore.getScoreLife().getScore());
    }

    @Test
    void shouldAddOneRiskPointToDisabilityAndLifeWhenHouseHasDependents() {
        House house = new House(OwnershipStatus.MORTGAGED);
        ReflectionTestUtils.setField(command, "age", 39);
        ReflectionTestUtils.setField(command, "income", 201000);
        ReflectionTestUtils.setField(command, "house", house);
        ReflectionTestUtils.setField(command, "dependents", 2);

        RiskScore riskScore = service.calculate(command);

        assertEquals(1, riskScore.getScoreAuto().getScore());
        assertEquals(3, riskScore.getScoreDisability().getScore());
        assertEquals(2, riskScore.getScoreHome().getScore());
        assertEquals(2, riskScore.getScoreLife().getScore());
    }

    @Test
    void shouldAddOneRiskPointToLifeAnDeductOneRiskPointToDisabilityWhenIsMarried() {
        House house = new House(OwnershipStatus.MORTGAGED);
        ReflectionTestUtils.setField(command, "age", 39);
        ReflectionTestUtils.setField(command, "income", 201000);
        ReflectionTestUtils.setField(command, "house", house);
        ReflectionTestUtils.setField(command, "dependents", 2);
        ReflectionTestUtils.setField(command, "maritalStatus", MaritalStatus.MARRIED);


        RiskScore riskScore = service.calculate(command);

        assertEquals(1, riskScore.getScoreAuto().getScore());
        assertEquals(2, riskScore.getScoreDisability().getScore());
        assertEquals(2, riskScore.getScoreHome().getScore());
        assertEquals(3, riskScore.getScoreLife().getScore());
    }

    @Test
    void shouldAddOneRiskPointToAutoWhenHasAVehicleProducedIn5Years() {
        House house = new House(OwnershipStatus.MORTGAGED);
        Vehicle vehicle = new Vehicle(Year.of(2019));
        ReflectionTestUtils.setField(command, "age", 39);
        ReflectionTestUtils.setField(command, "income", 201000);
        ReflectionTestUtils.setField(command, "house", house);
        ReflectionTestUtils.setField(command, "dependents", 2);
        ReflectionTestUtils.setField(command, "maritalStatus", MaritalStatus.MARRIED);
        ReflectionTestUtils.setField(command, "vehicle", vehicle);


        RiskScore riskScore = service.calculate(command);

        assertEquals(2, riskScore.getScoreAuto().getScore());
        assertEquals(2, riskScore.getScoreDisability().getScore());
        assertEquals(2, riskScore.getScoreHome().getScore());
        assertEquals(3, riskScore.getScoreLife().getScore());
    }

    @Test
    void testInputExample() {
        Integer[] risks = {0, 1, 0};
        Vehicle vehicle = new Vehicle(Year.of(2018));
        House house = new House(OwnershipStatus.OWNED);

        ReflectionTestUtils.setField(command, "age", 35);
        ReflectionTestUtils.setField(command, "dependents", 2);
        ReflectionTestUtils.setField(command, "house", house);
        ReflectionTestUtils.setField(command, "income", 0);
        ReflectionTestUtils.setField(command, "maritalStatus", MaritalStatus.MARRIED);
        ReflectionTestUtils.setField(command, "riskQuestions", risks);
        ReflectionTestUtils.setField(command, "vehicle", vehicle);


        RiskScore riskScore = service.calculate(command);

        assertEquals(1, riskScore.getScoreAuto().getScore());
        assertEquals(0, riskScore.getScoreDisability().getScore());
        assertEquals(0, riskScore.getScoreHome().getScore());
        assertEquals(2, riskScore.getScoreLife().getScore());
    }
}