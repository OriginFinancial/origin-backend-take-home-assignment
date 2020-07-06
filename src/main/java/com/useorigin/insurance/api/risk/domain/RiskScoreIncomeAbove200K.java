package com.useorigin.insurance.api.risk.domain;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;

public class RiskScoreIncomeAbove200K extends RiskDecorator {

    private static final int INCOME_ABOVE_200K_RISK_DEDUCTION = 1;
    private static final int INCOME_AMOUNT_RULE_200K = 200000;

    private Risk risk;

    public RiskScoreIncomeAbove200K(Risk risk) {
        this.risk = risk;
    }

    @Override
    public RiskScore createRiskScore(RiskProfileCreationCommand command) {
        if (command.getIncome() > INCOME_AMOUNT_RULE_200K)
            return updateRiskScore(risk.createRiskScore(command));
        return risk.createRiskScore(command);
    }

    @Override
    public RiskScore updateRiskScore(RiskScore riskScore) {
        return RiskScore.of(riskScore.getScoreAuto().deduct(INCOME_ABOVE_200K_RISK_DEDUCTION),
                riskScore.getScoreDisability().deduct(INCOME_ABOVE_200K_RISK_DEDUCTION),
                riskScore.getScoreHome().deduct(INCOME_ABOVE_200K_RISK_DEDUCTION),
                riskScore.getScoreLife().deduct(INCOME_ABOVE_200K_RISK_DEDUCTION));
    }
}
