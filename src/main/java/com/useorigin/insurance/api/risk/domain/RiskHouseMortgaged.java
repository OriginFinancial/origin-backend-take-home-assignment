//package com.useorigin.insurance.api.risk.domain;
//
//import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
//import com.useorigin.insurance.api.risk.domain.OwnershipStatus;
//import com.useorigin.insurance.api.risk.domain.RiskDecorator;
//import com.useorigin.insurance.api.risk.domain.RiskScore;
//import com.useorigin.insurance.api.risk.infrastructure.web.out.RiskProfileResource;
//
//public class RiskHouseMortgaged extends RiskDecorator {
//
//    private static final int HOUSE_MORTGAGED_RISK_ADDITION = 1;
//
//    public RiskHouseMortgaged(RiskDecorator risk) {
//        super(risk);
//    }
//
//    @Override
//    public RiskProfileResource createProfile(RiskProfileResource profile, RiskScore score, RiskProfileCreationCommand command) {
//
//        if (command.getHouse() != null) {
//            if (command.getHouse().getOwnershipStatus().equals(OwnershipStatus.MORTGAGED)) {
//                score = updateRiskScore(score);
//                profile = updateRiskProfile(profile);
//            }
//        }
//        return risk.createProfile(profile, score, command);
//    }
//
//    @Override
//    public RiskScore updateRiskScore(RiskScore defaultScore) {
//        return RiskScore.of(defaultScore.getScoreAuto(),
//                defaultScore.getScoreDisability().add(HOUSE_MORTGAGED_RISK_ADDITION),
//                defaultScore.getScoreHome().add(HOUSE_MORTGAGED_RISK_ADDITION),
//                defaultScore.getScoreLife());
//    }
//
//    @Override
//    public RiskProfileResource updateRiskProfile(RiskProfileResource defaultProfile) {
//        return null;
//    }
//}