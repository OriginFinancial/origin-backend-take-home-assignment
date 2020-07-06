package com.useorigin.insurance.api.risk.domain;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;

public class RiskScoreWithDependents extends RiskDecorator {

    private static final int DEPENDENTS_RISK_ADDITION = 1;
    private Risk risk;

    public RiskScoreWithDependents(Risk risk) {
        this.risk = risk;
    }

    @Override
    public RiskScore createRiskScore(RiskProfileCreationCommand command) {
        if (command.hasDependents())
            return updateRiskScore(risk.createRiskScore(command));
        return risk.createRiskScore(command);
    }

    @Override
    public RiskScore updateRiskScore(RiskScore riskScore) {
        return RiskScore.of(riskScore.getScoreAuto(),
                riskScore.getScoreDisability().add(DEPENDENTS_RISK_ADDITION),
                riskScore.getScoreHome(),
                riskScore.getScoreLife().add(DEPENDENTS_RISK_ADDITION));
    }
}
