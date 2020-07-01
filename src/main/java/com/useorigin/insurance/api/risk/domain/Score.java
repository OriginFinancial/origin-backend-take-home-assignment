package com.useorigin.insurance.api.risk.domain;

public interface Score {
    Integer deduct(Integer score);

    Integer add(Integer score);
}
