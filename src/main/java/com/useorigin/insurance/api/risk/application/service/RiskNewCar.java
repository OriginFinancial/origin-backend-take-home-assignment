package com.useorigin.insurance.api.risk.application.service;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
import com.useorigin.insurance.api.risk.domain.RiskDecorator;
import com.useorigin.insurance.api.risk.domain.RiskScore;
import com.useorigin.insurance.api.risk.infrastructure.web.out.RiskProfileResource;

import java.time.Year;

public class RiskNewCar extends RiskDecorator {

    private static final int VEHICLE_RISK_ADDITION = 1;

    @Override
    public RiskScore calculateScore(RiskScore score, RiskProfileCreationCommand command) {
        if (command.getVehicle() != null) {
            if (command.getVehicle().getYear().isAfter(Year.now().minusYears(5))) {
                RiskScore scoreMarried = createScoreVehicle(score);
                return scoreMarried;
            }
        }
        return score;
    }

    @Override
    public RiskProfileResource createProfile(RiskProfileResource profile, RiskProfileCreationCommand command) {
        return null;
    }

    private RiskScore createScoreVehicle(RiskScore defaultScore) {
        return RiskScore.of(defaultScore.getScoreAuto().add(VEHICLE_RISK_ADDITION),
                defaultScore.getScoreDisability(),
                defaultScore.getScoreHome(),
                defaultScore.getScoreLife());
    }
}
