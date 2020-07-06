package com.useorigin.insurance.api.risk.domain.service;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
import com.useorigin.insurance.api.risk.domain.MaritalStatus;
import com.useorigin.insurance.api.risk.domain.RiskScore;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.test.util.ReflectionTestUtils;

import static org.junit.jupiter.api.Assertions.assertEquals;

class RiskScoreServiceTest {

    RiskScoreService service = new RiskScoreService();
    private RiskProfileCreationCommand command;

    @BeforeEach
    void setup() {
        Integer[] risks = {1, 1, 1};

        command = new RiskProfileCreationCommand.Builder()
                .atAge(100)
                .withDependents(10)
                .withIncome(100000)
                .withMaritalStatus(MaritalStatus.MARRIED)
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
    void shouldDeductTOneRiskPointsWhenUserIsBetween30And40() {
        ReflectionTestUtils.setField(command, "age", 39);
        RiskScore riskScore = service.calculate(command);

        assertEquals(2, riskScore.getScoreAuto().getScore());
        assertEquals(2, riskScore.getScoreDisability().getScore());
        assertEquals(2, riskScore.getScoreHome().getScore());
        assertEquals(2, riskScore.getScoreLife().getScore());
    }

}