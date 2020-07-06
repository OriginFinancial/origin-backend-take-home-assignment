package com.useorigin.insurance.api.risk.domain;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;

import java.time.Year;

public class RiskScoreVehicleProducedIn5Years extends RiskDecorator {

    private static final int VEHICLE_RISK_ADDITION = 1;
    private static final Year LIMIT_YEAR_PRODUCTION = Year.now().minusYears(5);

    private Risk risk;

    public RiskScoreVehicleProducedIn5Years(Risk risk) {
        this.risk = risk;
    }

    @Override
    public RiskScore createRiskScore(RiskProfileCreationCommand command) {
        if (command.getVehicle() != null)
            if (command.getVehicle().getYear().isAfter(LIMIT_YEAR_PRODUCTION))
                return updateRiskScore(risk.createRiskScore(command));

        return risk.createRiskScore(command);
    }

    @Override
    public RiskScore updateRiskScore(RiskScore riskScore) {
        return RiskScore.of(riskScore.getScoreAuto().add(VEHICLE_RISK_ADDITION),
                riskScore.getScoreDisability(),
                riskScore.getScoreHome(),
                riskScore.getScoreLife());

    }
}
