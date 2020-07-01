package com.useorigin.insurance.api.risk.application.service;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
import com.useorigin.insurance.api.risk.domain.Risk;
import com.useorigin.insurance.api.risk.domain.RiskDecorator;
import com.useorigin.insurance.api.risk.domain.RiskScore;
import com.useorigin.insurance.api.risk.domain.ScoreType;
import com.useorigin.insurance.api.risk.infrastructure.web.out.RiskProfileResource;

public class RiskScoreIneligible extends RiskDecorator {


    public RiskScoreIneligible(Risk risk) {
        super(risk);
    }

    @Override
    public RiskScore calculateScore(RiskProfileCreationCommand command) {
        return null;
    }

    @Override
    public RiskProfileResource createProfile(RiskProfileResource riskProfileResource, RiskProfileCreationCommand command) {

        if (command.getIncome() == 0 && (command.getHouse() == null || command.getHouse() == null)) {
            RiskProfileResource profile = new RiskProfileResource(ScoreType.INELIGIBLE, ScoreType.INELIGIBLE, ScoreType.INELIGIBLE, ScoreType.INELIGIBLE);
            return risk.createProfile(profile, command);
        }

        return risk.createProfile(riskProfileResource, command);
    }
}
