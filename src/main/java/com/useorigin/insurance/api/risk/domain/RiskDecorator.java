package com.useorigin.insurance.api.risk.domain;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
import com.useorigin.insurance.api.risk.infrastructure.web.out.RiskProfileResource;

public abstract class RiskDecorator implements Risk {

    protected Risk risk;
    protected RiskScore riskScore;
    protected RiskProfileResource riskProfileResource;

    public RiskDecorator() {

    }

    public RiskDecorator(Risk risk) {
        this.risk = risk;
    }

    public RiskScore getRiskScore() {
        return riskScore;
    }

    public RiskProfileResource getRiskProfileResource() {
        return riskProfileResource;
    }

    public RiskProfileResource createProfile(RiskProfileCreationCommand command) {
        return risk.createProfile(riskProfileResource, command);
    }
}
