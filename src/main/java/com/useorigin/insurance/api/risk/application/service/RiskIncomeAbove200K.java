package com.useorigin.insurance.api.risk.application.service;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
import com.useorigin.insurance.api.risk.domain.Risk;
import com.useorigin.insurance.api.risk.domain.RiskDecorator;
import com.useorigin.insurance.api.risk.domain.RiskScore;
import com.useorigin.insurance.api.risk.infrastructure.web.out.RiskProfileResource;

public class RiskIncomeAbove200K extends RiskDecorator {

    private static final int INCOME_ABOVE_200K_RISK_DEDUCTION = 1;
    private static final int INCOME_AMOUNT_RULE_200K = 200000;

    public RiskIncomeAbove200K(Risk risk) {
        super(risk);
    }

    @Override
    public RiskScore calculateScore(RiskScore score, RiskProfileCreationCommand command) {

        if (command.getIncome() > INCOME_AMOUNT_RULE_200K) {
            RiskScore scoreIncomeAbove200K = createScoreIncomeAbove200K(score);
            return risk.calculateScore(scoreIncomeAbove200K, command);
        }

        return risk.calculateScore(score, command);
    }

    @Override
    public RiskProfileResource createProfile(RiskProfileResource profile, RiskProfileCreationCommand command) {
        return null;
    }

    private RiskScore createScoreIncomeAbove200K(RiskScore defaultScore) {
        return RiskScore.of(defaultScore.getScoreAuto().deduct(INCOME_ABOVE_200K_RISK_DEDUCTION),
                defaultScore.getScoreDisability().deduct(INCOME_ABOVE_200K_RISK_DEDUCTION),
                defaultScore.getScoreHome().deduct(INCOME_ABOVE_200K_RISK_DEDUCTION),
                defaultScore.getScoreLife().deduct(INCOME_ABOVE_200K_RISK_DEDUCTION));
    }
}
