package com.useorigin.insurance.api.risk.application.service;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
import com.useorigin.insurance.api.risk.domain.*;
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

        ScoreToScoreTypeConverter converter = new ScoreToScoreTypeConverter();
        RiskScore riskScore = riskScoreService.calculate(command);

        FindScoreTypeCommand autoCommand = new FindScoreTypeAuto(converter, riskScore.getScoreAuto());
        FindScoreTypeCommand disabilityCommand = new FindScoreTypeDisabilityCommand(converter, riskScore.getScoreDisability());
        FindScoreTypeCommand homeCommand = new FindScoreTypeHomeCommand(converter, riskScore.getScoreHome());
        FindScoreTypeCommand lifeCommand = new FindScoreTypeLifeCommand(converter, riskScore.getScoreLife());

        ScoreType autoScoreType = autoCommand.execute(command);
        ScoreType homeScoreType = homeCommand.execute(command);
        ScoreType disabilityScoreType = disabilityCommand.execute(command);
        ScoreType lifeScoreType = lifeCommand.execute(command);

        return new RiskProfileResource(autoScoreType, disabilityScoreType, homeScoreType, lifeScoreType);

    }

}
