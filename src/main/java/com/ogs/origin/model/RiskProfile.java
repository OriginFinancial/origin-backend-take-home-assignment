package com.ogs.origin.model;

import lombok.AllArgsConstructor;
import lombok.Getter;

@Getter
@AllArgsConstructor
public class RiskProfile {

    private Score life;

    private Score disability;

    private Score home;

    private Score auto;
}
