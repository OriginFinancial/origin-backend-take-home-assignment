package com.useorigin.insurance.api.risk.application.service;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
import com.useorigin.insurance.api.risk.domain.Risk;
import com.useorigin.insurance.api.risk.domain.RiskDecorator;
import com.useorigin.insurance.api.risk.domain.RiskScore;
import com.useorigin.insurance.api.risk.domain.ScoreType;
import com.useorigin.insurance.api.risk.infrastructure.web.out.RiskProfileResource;

public class RiskOver60 extends RiskDecorator {

    public RiskOver60() {
    }

    public RiskOver60(Risk risk) {
        super(risk);
    }

    @Override
    public RiskScore calculateScore(RiskScore score, RiskProfileCreationCommand command) {
        return risk.calculateScore(score, command);
    }

    @Override
    public RiskProfileResource createProfile(RiskProfileResource riskProfileResource, RiskProfileCreationCommand command) {
        if (command.getAge() > 60)
            return new RiskProfileResource(riskProfileResource.getAuto(),
                    ScoreType.INELIGIBLE,
                    riskProfileResource.getHome(),
                    ScoreType.INELIGIBLE);

        return riskProfileResource;
    }
}
