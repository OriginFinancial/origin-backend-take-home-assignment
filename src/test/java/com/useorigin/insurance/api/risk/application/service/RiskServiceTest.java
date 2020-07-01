package com.useorigin.insurance.api.risk.application.service;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
import com.useorigin.insurance.api.risk.domain.MaritalStatus;
import com.useorigin.insurance.api.risk.domain.ScoreType;
import com.useorigin.insurance.api.risk.infrastructure.web.out.RiskProfileResource;
import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.assertEquals;

class RiskServiceTest {


    @Test
    void shouldReturnIneligibleForAllInsuranceWhenHasNoCarNoHouseNoIncome() {

        RiskService riskService = new RiskService();
        RiskProfileResource profile = riskService.calculate(createPayloadRiskIneligible());

        assertEquals(ScoreType.INELIGIBLE, profile.getAuto());
        assertEquals(ScoreType.INELIGIBLE, profile.getDisability());
        assertEquals(ScoreType.INELIGIBLE, profile.getHome());
        assertEquals(ScoreType.INELIGIBLE, profile.getLife());

    }

    @Test
    void shouldReturnIneligibleForDisabilityAndLifeInsuranceWhenHasNoCarNoHouseNoIncome() {

        RiskService riskService = new RiskService();
        RiskProfileResource profile = riskService.calculate(createPayloadRiskIneligible());

        assertEquals(ScoreType.INELIGIBLE, profile.getDisability());
        assertEquals(ScoreType.INELIGIBLE, profile.getLife());

    }

    private RiskProfileCreationCommand createPayloadRiskIneligible() {

        Integer[] risks = {0, 1, 0};

        return new RiskProfileCreationCommand.Builder()
                .atAge(61)
                .withDependents(2)
                .withIncome(0)
                .withMaritalStatus(MaritalStatus.MARRIED)
                .withRisks(risks)
                .build();
    }


}