package com.useorigin.insurance.api.risk.domain;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;

public class RiskScoreBetween30To40 extends RiskDecorator {

    private static final int BETWEEN_30_AND_40_RISK_DEDUCTION = 1;
    private Risk risk;

    public RiskScoreBetween30To40(Risk risk) {
        this.risk = risk;
    }

    @Override
    public RiskScore createRiskScore(RiskProfileCreationCommand command) {
        if (command.getAge() >= 30 && command.getAge() < 40)
            return updateRiskScore(risk.createRiskScore(command));
        return risk.createRiskScore(command);
    }

    @Override
    public RiskScore updateRiskScore(RiskScore riskScore) {
        return RiskScore.of(riskScore.getScoreAuto().deduct(BETWEEN_30_AND_40_RISK_DEDUCTION),
                riskScore.getScoreDisability().deduct(BETWEEN_30_AND_40_RISK_DEDUCTION),
                riskScore.getScoreHome().deduct(BETWEEN_30_AND_40_RISK_DEDUCTION),
                riskScore.getScoreLife().deduct(BETWEEN_30_AND_40_RISK_DEDUCTION));
    }
}
