//package com.useorigin.insurance.api.risk.domain;
//
//import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
//import com.useorigin.insurance.api.risk.domain.RiskDecorator;
//import com.useorigin.insurance.api.risk.domain.RiskScore;
//import com.useorigin.insurance.api.risk.infrastructure.web.out.RiskProfileResource;
//
//public class RiskIncomeAbove200K extends RiskDecorator {
//
//    private static final int INCOME_ABOVE_200K_RISK_DEDUCTION = 1;
//    private static final int INCOME_AMOUNT_RULE_200K = 200000;
//
//    public RiskIncomeAbove200K(RiskDecorator risk) {
//        super(risk);
//    }
//
//    @Override
//    public RiskProfileResource createProfile(RiskProfileResource profile, RiskScore score, RiskProfileCreationCommand command) {
//
//        if (command.getIncome() > INCOME_AMOUNT_RULE_200K) {
//            score = updateRiskScore(score);
//            profile = updateRiskProfile(profile);
//        }
//        return risk.createProfile(profile, score, command);
//    }
//
//    @Override
//    public RiskScore updateRiskScore(RiskScore defaultScore) {
//        return RiskScore.of(defaultScore.getScoreAuto().deduct(INCOME_ABOVE_200K_RISK_DEDUCTION),
//                defaultScore.getScoreDisability().deduct(INCOME_ABOVE_200K_RISK_DEDUCTION),
//                defaultScore.getScoreHome().deduct(INCOME_ABOVE_200K_RISK_DEDUCTION),
//                defaultScore.getScoreLife().deduct(INCOME_ABOVE_200K_RISK_DEDUCTION));
//    }
//
//    @Override
//    public RiskProfileResource updateRiskProfile(RiskProfileResource defaultProfile) {
//        return null;
//    }
//}
