package com.useorigin.insurance.api.risk.domain;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;

public interface FindScoreTypeCommand {
    ScoreType execute(RiskProfileCreationCommand command);
}
