import random
import time
import requests

cookies = {
    'MVID_REGION_ID': '1',
    'MVID_CITY_ID': 'CityCZ_975',
    'MVID_TIMEZONE_OFFSET': '3',
    'MVID_KLADR_ID': '7700000000000',
    'MVID_REGION_SHOP': 'S002',
    'MVID_NEW_LK_OTP_TIMER': 'true',
    'MVID_CHAT_VERSION': '6.26.0',
    'SENTRY_TRANSACTIONS_RATE': '0.4',
    'SENTRY_REPLAYS_SESSIONS_RATE': '0.4',
    'SENTRY_REPLAYS_ERRORS_RATE': '0.4',
    'SENTRY_ERRORS_RATE': '0.4',
    'MVID_FILTER_CODES': 'true',
    'MVID_SUGGEST_DIGINETICA': 'true',
    'MVID_MEDIA_STORIES': 'true',
    'MVID_FLOCKTORY_ON': 'true',
    'MVID_ENABLED_CACHE_PRODUCTSETS': 'true',
    'MVID_SERVICES': '111',
    'MVID_IS_NEW_BR_WIDGET': 'true',
    'MVID_NEW_LK_CHECK_CAPTCHA': 'true',
    'MVID_GTM_ENABLED': '011',
    'MVID_CRITICAL_GTM_INIT_DELAY': '1500',
    'MVID_WEB_SBP': 'true',
    'MVID_CREDIT_SERVICES': 'true',
    'MVID_TYP_CHAT': 'true',
    'MVID_SP': 'true',
    'MVID_CASCADE_CMN': 'true',
    'MVID_AB_UPSALE': 'true',
    'MVID_AB_PERSONAL_RECOMMENDS': 'true',
    'MVID_SERVICE_AVLB': 'true',
    'MVID_TYP_ACCESSORIES_ORDER_SET': 'true',
    'MVID_GROUP_BY_QUALITY': 'true',
    'MVID_DISPLAY_PERS_DISCOUNT': 'true',
    'MVID_AB_PERSONAL_RECOMMENDS_SRP': 'true',
    'MVID_DIGINETICA_ENABLED': 'true',
    'MVID_ACCESSORIES_ORDER_SET_VERSION': '1',
    'MVID_BYPASS_FC': 'true',
    'MVID_IMG_RESIZE': 'true',
    'MVID_NEW_GET_SHOPPING_CART_SHORT': 'true',
    'MVID_WEB_QR': 'true',
    'MVID_SRP_DIGINETICA_ENABLED': 'true',
    'MVID_SORM_INTEGRATION': 'true',
    'MVID_QUASAR_CUSTOMER_V2': 'false',
    'MVID_QUASAR_UPDATE_CUSTOMER_V2': 'false',
    'MVID_MOBWEB_NEW_MAIN': 'true',
    'MVID_APLAUT_REVIEWS': 'true',
    'MVID_BR_WRITE_OFF': '50',
    'MVID_ANALYTIC_SERVICE_EMPTY_GA_ID': 'true',
    'MVID_PROMO_20': 'true',
    'MVID_DISABLEDITEM_PRICE': 'true',
    'MVID_CHECKOUT_V2': 'true',
    'MVID_ABTEST_RECOMMENDATION': 'DATAOFFICE2',
    'MVID_SENTRY_ENABLED': 'false',
    'MVID_ENVCLOUD': 'prod2',
    'MVID_DEVICE_UUID': '68f4430a-2418-4008-ae73-7a6d5c5f4b83',
    '_ym_uid': '1760807304179326209',
    '_ym_d': '1760807304',
    '__SourceTracker': 'google__organic',
    'admitad_deduplication_cookie': 'google__organic',
    'tmr_lvid': 'c0c83397332e4a161e1328f04411e07a',
    'tmr_lvidTS': '1760807304487',
    'flocktory-uuid': '915a8a23-e597-4079-89f5-d5d1e19ee902-0',
    'advcake_track_id': '5cdbc8ec-66bf-92e3-f8ed-696b6df93ad2',
    'advcake_session_id': '045a0de1-1539-b21a-857e-c757980e7f4a',
    'afUserId': 'a97f0bd6-b582-4d77-add3-18c599e378ea-p',
    'AF_SYNC': '1760807304968',
    'flacktory': 'no',
    'BIGipServeratg-ps-prod_tcp80': '2969885706.20480.0000',
    'bIPs': '-971835924',
    '_userGUID': '0:mgwj7gh3:IJipyfzSFTKtqqMSl9plsiuFQPyL5J3S',
    'mindboxDeviceUUID': '4c44e660-30da-4050-be7a-1ab984e2df13',
    'directCrm-session': '%7B%22deviceGuid%22%3A%224c44e660-30da-4050-be7a-1ab984e2df13%22%7D',
    '_userGUID': '0:mgwj7gh3:IJipyfzSFTKtqqMSl9plsiuFQPyL5J3S',
    'MVID_GEOLOCATION_NEEDED': 'false',
    '__lhash_': '62c1cc4c249a845448c0e08c76dd6a72',
    '__hash_': '0e75725888479613279372d974a613f4',
    'MVID_CREDIT_BUTTON_CART_D': 'false_2',
    'MVID_CREDIT_BUTTON_CART_M': 'true_2',
    'MVID_ADD_CATALOG_BUTTON': 'true',
    '_sp_ses.d61c': '*',
    '_ym_isad': '1',
    'domain_sid': 'RKdrJcqCgZIRZHN9dTm5p%3A1761172125177',
    'dSesn': '6559f5d6-2258-dca0-2f71-97123c6db8ad',
    '_dvs': '0:mh2keski:Y9l0qaxDx2v3V_o7qZVXhDziqjjeUI62',
    'tmr_detect': '1%7C1761172128137',
    'advcake_track_url': '%3D20250113SEeSt6H8nakrbhRHlnx5z66T0LC0VeeoEn6zLKtVi7bBLIKvEdLCiWLy75K2eAP0fdw5cDurE9d9MnRC2xAxdwZhjYWU1F174BJuthNsNaMQbBOhMUmG9HG2NnUrtLvybPLjkdwIIzS%2F7zExU%2FBpmJzD5qEVV0DnYVKaBXKb%2FTDqkUa9e8x956PmtgeF5thfbUxLqPFevL9Nyp%2Fo1mrAcY3Q6VV9pFHyF812CjgZuObBJQ4kTnF%2BUJDFTDJBpkGrjkdZqp7uy78kwdHvB%2BOiI4L7ZrxK6Jka49%2F%2Bv5k5LGomRL%2BDlDOCYnupQlRlU5Dypk4gRv6emlFYR4swYvpzBa3TTq4EBEW9bzWVP6qc76CrznsKAgxFUxq11RGxdIHSD234iEHgEWGfDJTnxWtDcvW8XwZ0qw4MSZuu7A5xRLxwa9wf7i7d2qMGAF9FtVLvyivAQs5UCZL5FEIJxDc4pNo8rnowAIQ3AvaMvQDCID%2BkWFxZlnvR9Ei1ptnPG%2Bh0eXG8MZ2mScurE8qywOzc2J%2FndRhsdXS%2BbtoKsLqVd%2Fk4P6PPpvgdQKI7%2BOP%2FzKlkBulASEH%2BtFYxO2qN7MChdcWEPWNdrMdAXAW%2F3gPcKBakcwJwfFmAPXM6NSOBFURsWY5SRPaIf%2B1Hn0%2B%2BHvkgYDOk%2BCbv7r%2FnTJCcJvliUo3bHjmCOVF4doU%3D',
    'digi_uc': '|v:176088:4204958!176117:20040495|c:176080:400102758!176088:20060271:400306377:20078449:400301805!176117:20040495',
    'gsscgib-w-mvideo': 'n1T3PPaVtDkWVmxZvOa5rEgdq1u5WPw6m7v1xbrtvUEhZtFkkyH5LG/tMzZeezTXCUgXSNA2U90wqa1zOfZVJZn5R3oJAf7N/VKDBCldW5XEaiOuhocIbGtQSlUeGbZPlW66uMOwpf9Wji1XW3nDEodBxEH+ZpDRI9At6vFyBN6w9qvLNWZi6BhECrqtjbW6yMVrU9bUBBA86ajWez3FSrIQew3LAhHQ5eAD9UyMmT96P+kjjbnqrtcgIpKYFw==',
    'gsscgib-w-mvideo': 'n1T3PPaVtDkWVmxZvOa5rEgdq1u5WPw6m7v1xbrtvUEhZtFkkyH5LG/tMzZeezTXCUgXSNA2U90wqa1zOfZVJZn5R3oJAf7N/VKDBCldW5XEaiOuhocIbGtQSlUeGbZPlW66uMOwpf9Wji1XW3nDEodBxEH+ZpDRI9At6vFyBN6w9qvLNWZi6BhECrqtjbW6yMVrU9bUBBA86ajWez3FSrIQew3LAhHQ5eAD9UyMmT96P+kjjbnqrtcgIpKYFw==',
    '_sp_id.d61c': 'e5a953d3-c77c-466d-a344-1a9248f4823a.1760807304.5.1761172135.1760887385.3c70bcc2-ddaf-4393-96ea-6828747ead1b.594ef378-6905-4d69-b7ce-d767bac81326.120202bc-2fa3-4b00-9437-3b3073feb733.1761172123608.12',
    'fgsscgib-w-mvideo': 'RiwBdbebc16851b7273522d36b9b673f9144e488',
    'fgsscgib-w-mvideo': 'RiwBdbebc16851b7273522d36b9b673f9144e488',
    'gsscgib-w-mvideo': 'gAWHvB2fB9BnwXI55TD8kBFjotC8AnqugZX4mUutFWbk7DBqxU5G1umdJAoVhCVYQzsqyS+FXD0AmR3viFBh1tMPo8M3udWdHaOdYh0yEVxL8vnRGeLAnl0n2MsuuDp33xSifOtk1DO9z6rxBrT/HJ8elfQ4w8hgtvQRX+uVgf8skRSbQ4QcViODWhTlw4ISXeUPAC3FHQZtodolUuEr69ycdJ0O93tUcmf6CE9R9lA+OsGJR7VoDItNnGNzBg==',
    'cfidsgib-w-mvideo': 'SKjKvUZNxZGeyxjgk3ayp515BgYA5cYlbLeoDUv1JTX8EvwsrWsi14CEyoS30WyY+2O+cwKbfTVE7i6mIsGRkBQW2Ei3zcsGn8Znp8mo+cTvs6AWfDg4u3FvBQTy4bRzIfcfuG8VqKrhEBNWGb4LUr5c5yBUAv0mPxuo9MY=',
}

headers = {
    'accept': '*/*',
    'accept-language': 'en-US,en;q=0.9,ru;q=0.8,ru-RU;q=0.7',
    'priority': 'u=1, i',
    'referer': 'https://www.mvideo.ru/products/elektrochainik-tefal-element-steel-ki280d30-20040495',
    'sec-ch-ua': '"Google Chrome";v="141", "Not?A_Brand";v="8", "Chromium";v="141"',
    'sec-ch-ua-mobile': '?0',
    'sec-ch-ua-platform': '"Windows"',
    'sec-fetch-dest': 'empty',
    'sec-fetch-mode': 'cors',
    'sec-fetch-site': 'same-origin',
    'user-agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/141.0.0.0 Safari/537.36',
}

def get_params(product_id: str):
  return { 'productId': product_id }

all_teapots: list[str] = []
with open('all_linkns.txt') as f:
  all_teapots = f.readlines()

all_teapots = [id[:-1] for id in all_teapots]

print(all_teapots)

for i, teapot in enumerate(all_teapots):
    response = requests.get('https://www.mvideo.ru/bff/product-details', params=get_params(teapot), cookies=cookies, headers=headers)
    with open(f'pages/{i}.json', 'w') as f:
       f.write(response.text)
    print(f"{i}'th request: {response.status_code}")
    time.sleep(random.expovariate(0.5))
