package com.useorigin.insurance.api.risk.domain;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;

public class RiskWithDependents extends RiskDecorator {

    private static final int DEPENDENTS_RISK_ADDITION = 1;

    @Override
    public RiskScore createRiskScore(RiskProfileCreationCommand command) {
        return null;
    }

    @Override
    public RiskScore updateRiskScore(RiskScore riskScore) {
        return null;
    }
}
