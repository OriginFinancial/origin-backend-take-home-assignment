package com.useorigin.insurance.api.risk.domain;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;

public class RiskIsMarried extends RiskDecorator {

    private static final int MARRIED_RISK_ADDITION = 1;
    private static final int MARRIED_RISK_DEDUCTION = 1;

    @Override
    public RiskScore createRiskScore(RiskProfileCreationCommand command) {
        return null;
    }

    @Override
    public RiskScore updateRiskScore(RiskScore riskScore) {
        return null;
    }
}
