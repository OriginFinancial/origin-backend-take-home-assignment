package com.useorigin.insurance.api.risk.domain;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;

public class FindScoreTypeHomeCommand implements FindScoreTypeCommand {

    private final ScoreToScoreTypeConverter converter;
    private final Score score;

    public FindScoreTypeHomeCommand(ScoreToScoreTypeConverter converter, Score score) {
        this.converter = converter;
        this.score = score;
    }

    @Override
    public ScoreType execute(RiskProfileCreationCommand command) {
        if (command.getHouse() == null)
            return ScoreType.INELIGIBLE;

        return converter.convert(score.getValue());
    }
}
