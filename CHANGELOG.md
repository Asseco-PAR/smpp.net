# Changelog
 
All notable changes to this fork will be documented in this file.
 
This is a fork of [markjulmar/smpp.net](https://github.com/markjulmar/smpp.net)
(last upstream commit: `51bad848b658fe42e7f427a85dcc1e6a0ab6f9bf`, MIT License). Changes made
downstream of that point are documented below.

## [1.2.1] - 2026-04-27
### Fixed
- Late SMPP responses no longer crash the session. When a response arrives after its request was already timed out or purged,
  the response is silently discarded instead of throwing SmppException and closing the socket.

---

## [1.2.0] - 2026-04-16
### Fixed
- Pending request slots are now always released via try/finally, preventing zombie entries when SendPdu or WaitForResponse throws.
- PendingRequestLimit property now actually controls the limit (was comparing against hardcoded constant of 10).
- SmscSession.DeliverSm: added missing null check and WaitForResponse.

### Added
- Background stale-entry reaper timer (purges zombies older than 2x timeout).
- ClearPendingRequests() method for manual queue reset.
- PendingRequestCount property for diagnostics.

### Changed
- Default pending request limit raised from 10 to 50.

---

## [1.1.0] - 2026-04-16
### Added
- `Utility/EncodingHelper.cs` - maps SMPP `DataEncoding` values to
  `System.Text.Encoding` instances (`UCS2`/`BigEndianUnicode`,`IA5`/`ASCII`, `CYRLLIC`/`ISO-8859-5`, `SMSC_DEFAULT`/`ISO-8859-1` for backwards compatibility, etc.).
- `DataCoding` property on `short_message` and `message_payload` elements,
  so they know which encoding to use when serialising/deserialising.
- `message_payload.SetText(string, DataEncoding)` convenience helper.

### Changed
- `SmppOctetString.AddToStream` and `GetFromStream` are now `virtual` so that `short_message` can override them.
- `submit_sm` / `submit_sm_multi` / `deliver_sm` / `broadcast_sm` / `data_sm` propagate their `data_coding` to the backing `msg_` / `msgPayload_` elements before serialisation and after deserialisation.
- The short/long-message branching in the `Message` setter now uses the encoded byte count in the selected encoding, not `string.Length`.

## [1.0.0] - 2026-03-25
### Changed
- Migration of the SMPP library to .NETStandard 2.0 framework 