package com.useorigin.insurance.api.risk.domain;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;

public class RiskScoreMarried extends RiskDecorator {

    private static final int MARRIED_RISK_ADDITION = 1;
    private static final int MARRIED_RISK_DEDUCTION = 1;

    private Risk risk;

    public RiskScoreMarried(Risk risk) {
        this.risk = risk;
    }

    @Override
    public RiskScore createRiskScore(RiskProfileCreationCommand command) {
        if (command.getMaritalStatus().equals(MaritalStatus.MARRIED))
            return updateRiskScore(risk.createRiskScore(command));
        return risk.createRiskScore(command);
    }

    @Override
    public RiskScore updateRiskScore(RiskScore riskScore) {
        return RiskScore.of(riskScore.getScoreAuto(),
                riskScore.getScoreDisability().deduct(MARRIED_RISK_DEDUCTION),
                riskScore.getScoreHome(),
                riskScore.getScoreLife().add(MARRIED_RISK_ADDITION));
    }
}