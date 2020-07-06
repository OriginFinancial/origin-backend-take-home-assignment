package com.useorigin.insurance.api.risk.application.service;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
import com.useorigin.insurance.api.risk.domain.ScoreType;
import com.useorigin.insurance.api.risk.domain.service.RiskScoreService;
import com.useorigin.insurance.api.risk.infrastructure.web.out.RiskProfileResource;
import org.springframework.stereotype.Service;

@Service
public class RiskProfileService {

    private final RiskScoreService riskScoreService;

    public RiskProfileService(RiskScoreService riskScoreService) {
        this.riskScoreService = riskScoreService;
    }

    public RiskProfileResource createProfile(RiskProfileCreationCommand command) {

        RiskProfileResource riskProfileResource = new RiskProfileResource();

        if (isIneligibleProfile(command))
            return new RiskProfileResource(ScoreType.INELIGIBLE, ScoreType.INELIGIBLE, ScoreType.INELIGIBLE, ScoreType.INELIGIBLE);

        if (command.getAge() > 60)
            riskProfileResource = riskProfileResource.updateDisability(ScoreType.INELIGIBLE).updateLife(ScoreType.INELIGIBLE);

        return riskProfileResource;
    }

    private boolean isIneligibleProfile(RiskProfileCreationCommand command) {
        return command.getIncome() <= 0 || command.getVehicle() == null || command.getHouse() == null;
    }

}
