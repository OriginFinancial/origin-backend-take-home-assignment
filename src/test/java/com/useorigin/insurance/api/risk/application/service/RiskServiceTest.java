package com.useorigin.insurance.api.risk.application.service;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
import com.useorigin.insurance.api.risk.domain.*;
import com.useorigin.insurance.api.risk.infrastructure.web.out.RiskProfileResource;
import org.junit.jupiter.api.Test;

import java.time.Year;

import static org.junit.jupiter.api.Assertions.assertEquals;

class RiskServiceTest {

    private final RiskService riskService = new RiskService();

    @Test
    void shouldReturnIneligibleForAllInsuranceWhenHasNoCarNoHouseNoIncome() {
        RiskProfileResource profile = riskService.createProfile(createPayloadRiskIneligible());

        assertEquals(ScoreType.INELIGIBLE, profile.getAuto());
        assertEquals(ScoreType.INELIGIBLE, profile.getDisability());
        assertEquals(ScoreType.INELIGIBLE, profile.getHome());
        assertEquals(ScoreType.INELIGIBLE, profile.getLife());
    }

    @Test
    void shouldReturnIneligibleForDisabilityAndLifeInsuranceWhenHasNoCarNoHouseNoIncome() {
        RiskProfileResource profile = riskService.createProfile(createPayloadRiskIneligible());

        assertEquals(ScoreType.INELIGIBLE, profile.getDisability());
        assertEquals(ScoreType.INELIGIBLE, profile.getLife());
    }

    @Test
    void shouldDeductTwoRiskPointsWhenUserIsUnder30() {
        RiskProfileCreationCommand payloadRiskUnder30 = createPayloadRiskUnder30();
        RiskScore score = riskService.calculateScore(payloadRiskUnder30);

        assertEquals(-1, score.getScoreAuto().getScore());
        assertEquals(-1, score.getScoreDisability().getScore());
        assertEquals(-1, score.getScoreHome().getScore());
        assertEquals(1, score.getScoreLife().getScore());

        System.out.println(score);
    }

    @Test
    void shouldDeductOneRiskPointWhenUserIsBetween30And40() {
        RiskProfileCreationCommand payloadRiskBetween30And40 = createPayloadRiskBetween30And40();
        RiskScore score = riskService.calculateScore(payloadRiskBetween30And40);

        assertEquals(1, score.getScoreAuto().getScore());
        assertEquals(0, score.getScoreDisability().getScore());
        assertEquals(1, score.getScoreHome().getScore());
        assertEquals(2, score.getScoreLife().getScore());
    }

    @Test
    void shouldDeductOneRiskPointWhenIncomeIsAbove200K() {
        RiskProfileCreationCommand payloadRiskIncomeAbove200K = createPayloadIncomeAbove200K();
        RiskScore score = riskService.calculateScore(payloadRiskIncomeAbove200K);

        assertEquals(1, score.getScoreAuto().getScore());
        assertEquals(0, score.getScoreDisability().getScore());
        assertEquals(1, score.getScoreHome().getScore());
        assertEquals(2, score.getScoreLife().getScore());
    }

    @Test
    void shouldAddOneRiskPointToHomeAndDisabilityInsuranceWhenHouseIsMortgaged() {
        RiskProfileCreationCommand payloadRiskHouseIsMortgaged = createPayloadHouseIsMortgaged();
        RiskScore score = riskService.calculateScore(payloadRiskHouseIsMortgaged);

        assertEquals(2, score.getScoreAuto().getScore());
        assertEquals(2, score.getScoreDisability().getScore());
        assertEquals(3, score.getScoreHome().getScore());
        assertEquals(3, score.getScoreLife().getScore());
    }

    @Test
    void shouldAddOneRiskPointToDisabilityAndLifeInsuranceWhenHaveDependents() {
        RiskProfileCreationCommand payloadRiskDependents = createPayloadDependents();
        RiskScore score = riskService.calculateScore(payloadRiskDependents);

        assertEquals(2, score.getScoreAuto().getScore());
        assertEquals(4, score.getScoreDisability().getScore());
        assertEquals(3, score.getScoreHome().getScore());
        assertEquals(3, score.getScoreLife().getScore());
    }

    @Test
    void shouldAddOneRiskPointToLifeAndDeductOneRiskPointToDisabilityWhenIsMarried() {
        RiskProfileCreationCommand payloadRiskMarried = createPayloadMarried();
        RiskScore score = riskService.calculateScore(payloadRiskMarried);

        assertEquals(2, score.getScoreAuto().getScore());
        assertEquals(3, score.getScoreDisability().getScore());
        assertEquals(3, score.getScoreHome().getScore());
        assertEquals(4, score.getScoreLife().getScore());
    }

    @Test
    void shouldAddOneRiskPointToAutoInsuranceWhenCarProducedLastFiveYears() {
        RiskProfileCreationCommand payloadRiskNewCar = createPayloadNewCar();
        RiskScore score = riskService.calculateScore(payloadRiskNewCar);

        assertEquals(3, score.getScoreAuto().getScore());
        assertEquals(3, score.getScoreDisability().getScore());
        assertEquals(3, score.getScoreHome().getScore());
        assertEquals(4, score.getScoreLife().getScore());
    }

    private RiskProfileCreationCommand createPayloadRiskIneligible() {

        Integer[] risks = {2, 1, 0};

        return new RiskProfileCreationCommand.Builder()
                .atAge(61)
                .withDependents(0)
                .withIncome(0)
                .withMaritalStatus(MaritalStatus.MARRIED)
                .withRisks(risks)
                .build();
    }

    private RiskProfileCreationCommand createPayloadRiskUnder30() {

        Integer[] risks = {0, 1, 0};

        return new RiskProfileCreationCommand.Builder()
                .atAge(20)
                .withDependents(2)
                .withIncome(0)
                .withMaritalStatus(MaritalStatus.MARRIED)
                .withRisks(risks)
                .build();
    }

    private RiskProfileCreationCommand createPayloadRiskBetween30And40() {

        Integer[] risks = {1, 1, 0};

        return new RiskProfileCreationCommand.Builder()
                .atAge(30)
                .withDependents(0)
                .withIncome(0)
                .withMaritalStatus(MaritalStatus.MARRIED)
                .withRisks(risks)
                .build();
    }

    private RiskProfileCreationCommand createPayloadIncomeAbove200K() {

        Integer[] risks = {1, 1, 0};

        return new RiskProfileCreationCommand.Builder()
                .atAge(55)
                .withDependents(0)
                .withIncome(210000)
                .withMaritalStatus(MaritalStatus.MARRIED)
                .withRisks(risks)
                .build();
    }

    private RiskProfileCreationCommand createPayloadHouseIsMortgaged() {

        Integer[] risks = {1, 1, 1};
        House house = new House(OwnershipStatus.MORTGAGED);

        return new RiskProfileCreationCommand.Builder()
                .atAge(50)
                .withDependents(0)
                .withIncome(300000)
                .withMaritalStatus(MaritalStatus.MARRIED)
                .withRisks(risks)
                .withHouse(house)
                .build();
    }

    private RiskProfileCreationCommand createPayloadDependents() {

        Integer[] risks = {1, 1, 1};
        House house = new House(OwnershipStatus.MORTGAGED);

        return new RiskProfileCreationCommand.Builder()
                .atAge(50)
                .withDependents(2)
                .withIncome(300000)
                .withMaritalStatus(MaritalStatus.SINGLE)
                .withRisks(risks)
                .withHouse(house)
                .build();
    }

    private RiskProfileCreationCommand createPayloadMarried() {

        Integer[] risks = {1, 1, 1};
        House house = new House(OwnershipStatus.MORTGAGED);

        return new RiskProfileCreationCommand.Builder()
                .atAge(50)
                .withDependents(2)
                .withIncome(300000)
                .withMaritalStatus(MaritalStatus.MARRIED)
                .withRisks(risks)
                .withHouse(house)
                .build();
    }

    private RiskProfileCreationCommand createPayloadNewCar() {

        Integer[] risks = {1, 1, 1};
        House house = new House(OwnershipStatus.MORTGAGED);
        Vehicle vehicle = new Vehicle(Year.of(2018));

        return new RiskProfileCreationCommand.Builder()
                .atAge(50)
                .withDependents(2)
                .withIncome(300000)
                .withMaritalStatus(MaritalStatus.MARRIED)
                .withRisks(risks)
                .withHouse(house)
                .withCar(vehicle)
                .build();
    }
}