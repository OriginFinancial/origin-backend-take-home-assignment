package com.useorigin.insurance.api.risk.domain;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;

public class RiskScoreUnder30 extends RiskDecorator {

    private static final int UNDER_30_RISK_DEDUCTION = 2;
    private Risk risk;

    public RiskScoreUnder30(Risk risk) {
        this.risk = risk;
    }

    @Override
    public RiskScore createRiskScore(RiskProfileCreationCommand command) {
        if (command.getAge() < 30)
            return updateRiskScore(risk.createRiskScore(command));
        return risk.createRiskScore(command);
    }

    @Override
    public RiskScore updateRiskScore(RiskScore riskScore) {
        return RiskScore.of(riskScore.getScoreAuto().deduct(UNDER_30_RISK_DEDUCTION),
                riskScore.getScoreDisability().deduct(UNDER_30_RISK_DEDUCTION),
                riskScore.getScoreHome().deduct(UNDER_30_RISK_DEDUCTION),
                riskScore.getScoreLife().deduct(UNDER_30_RISK_DEDUCTION));
    }
}