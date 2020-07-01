package com.useorigin.insurance.api.risk.application.service;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
import com.useorigin.insurance.api.risk.domain.RiskDecorator;
import com.useorigin.insurance.api.risk.infrastructure.web.out.RiskProfileResource;
import org.springframework.stereotype.Service;

@Service
public class RiskService {

    public RiskProfileResource calculate(RiskProfileCreationCommand command) {

        Integer score = command.getRiskScore();

        RiskProfileResource profile = new RiskProfileResource();
        RiskDecorator risk = new RiskScoreIneligible(new RiskOver60());

        return risk.createProfile(profile, command);

    }
}
