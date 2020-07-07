package com.useorigin.insurance.api.risk.domain;

import org.springframework.core.convert.converter.Converter;
import org.springframework.stereotype.Component;

@Component
public class ScoreToScoreTypeConverter implements Converter<Integer, ScoreType> {

    @Override
    public ScoreType convert(Integer source) {
        if (source <= 0)
            return ScoreType.ECONOMIC;

        if (source <= 2)
            return ScoreType.REGULAR;

        return ScoreType.RESPONSIBLE;
    }
}
